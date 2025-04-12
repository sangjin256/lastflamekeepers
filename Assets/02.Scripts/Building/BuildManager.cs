using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildManager : BehaviourSingleton<BuildManager>
{
    [Header("빌딩 프리팹")]
    public List<GameObject> BuildingPrefabs;        // 배치할 건물 프립팹
    private ABaseBuilding _previewBuilding = null;       // 프리뷰용 건물
    private BoxCollider2D _previewBuildingCollider = null;

    public bool IsCreateMode = false;

    [Header("프리뷰 건물 색")]
    public Color CanBuildColor = Color.white;
    public Color CannotBuildColor = Color.red;

    public Action<ABaseBuilding> OnChangeBuildingList;
    public Action<ABaseBuilding> OnChangeBuildingActive;


    // 소환된 건물들
    private PriorityQueue<ABaseBuilding, float> _buildingPriorityQueue;                                     // 활성화 된 건물 관리 우선순위 큐
    private Stack<(ABaseBuilding, float)> _disabledBuildingStack;                                           // 비활성화 된 건물 관리 스택
    private Dictionary<BuildingType, List<ABaseBuilding>> _buildingDicList;                                 // 전체 건물 타입별 레벨 별 딕셔너리
    private bool _isLaboratoryBuilded = false;
    public bool IsCompleteSelectForgeToolType = true;
    
    // 건물별 데이터 리스트
    private ReadOnlyList<BuildData> _buildDataList;
    private Dictionary<BuildingType, BuildData> _buildDataDic;

    // 건물 설치 비용
    private Dictionary<BuildingType, List<int>> _requiredResourcesDicList;

    private List<string> _compareTapStringList = new List<string>{ "Fire", "Water", "Resource", "Unit", "Enemy"};

    private void Start()
    {
        // 데이터 받아오기
        Global.Instance.OnDataLoaded += _LoadBuildData;

        // FireManager 범위 변경 이벤트 구독
        FireManager.Instance.OnFireRangeChanged += UpdateFireRange;

        _isLaboratoryBuilded = false;
    }


    public void _LoadBuildData()
    {
        _buildDataList = DataTable.Instance.GetBuildDataList();

        // BuildData를 Dictionary로 저장
        _buildDataDic = new Dictionary<BuildingType, BuildData>();
        _requiredResourcesDicList = new Dictionary<BuildingType, List<int>>();

        foreach (BuildData buildData in _buildDataList)
        {
            _buildDataDic[buildData.BuildingType] = buildData;
            _requiredResourcesDicList[buildData.BuildingType] = new List<int> { buildData.WoodCount, buildData.StoneCount };
        }

        Initialize();
    }

    private void Initialize()
    {
        // 우선순위 큐 초기화
        _buildingPriorityQueue = new PriorityQueue<ABaseBuilding, float>();

        // 딕셔너리 리스트 초기화
        _buildingDicList = new Dictionary<BuildingType, List<ABaseBuilding>>();
        // TODO : 건물 추가 시 BuildingType.Count 추가 및 수정
        Debug.Log("[박우영]BuildingType Enum의 마지막이 Forge인지 확인하시오");
        for (int i = 0; i <= (int)BuildingType.Forge; i++)
        {
            // 레벨의 크기만큼 리스트 초기화
            int upgradeCount = _buildDataList[0].Upgrade_AddValueList.Count + 1;
            List<ABaseBuilding> buildingList = new List<ABaseBuilding>();

            _buildingDicList[(BuildingType)i] = buildingList;
        }

        // 스택 초기화
        _disabledBuildingStack = new Stack<(ABaseBuilding, float)>();

        //// ****************** 테스트 용 ******************
        //InventoryResourceManager.Instance.TryAddMaxResourceCount(InventoryResourceType.Wood, 300);
        //InventoryResourceManager.Instance.TryAddMaxResourceCount(InventoryResourceType.Stone, 300);
        //InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Wood, 300);
        //InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Stone, 300);
        //// **********************************************
    }

    [ContextMenu("AddResource")]
    public void AddResourceTest()
    {
        InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Wood, 100);
        InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Stone, 100);
        InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Ash, 100);
    }

    private void Update()
    {
        // 현재 건물 짓는 모드인지 확인
        if (_previewBuilding == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            // 취소하면 소모 자원 반환
            InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Wood, _requiredResourcesDicList[_previewBuilding.BuildingType][0]);
            InventoryResourceManager.Instance.TryAddCurrentResourceCount(InventoryResourceType.Stone, _requiredResourcesDicList[_previewBuilding.BuildingType][1]);
            EndBuildingMode();
            return;
        }
        
        // 마우스에 따라서 프리뷰 움직이기
        MovePreviewBuildingToMouse();

        // 건물 지을 수 있는 지 확인
        if (CheckBuildCondition())
        { 
            _previewBuilding.ChangeColor(CanBuildColor);
            if (Input.GetMouseButtonDown(0))
            {
                ConstructBuilding();
            }
            
        }
        else
        {
            _previewBuilding.ChangeColor(CannotBuildColor);
        }
    }

    private void MovePreviewBuildingToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _previewBuilding.transform.position = mousePosition - _previewBuildingCollider.offset;
    }

    private bool CheckBuildCondition()
    {
        // 1. 다른 건물과 충돌 여부 확인
        Vector2 centerOfBuilding = (Vector2)_previewBuilding.transform.position + _previewBuildingCollider.offset;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerOfBuilding, _previewBuildingCollider.size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Building"))
            {
                ABaseBuilding building = collider.GetComponent<ABaseBuilding>();
                if (building && building != _previewBuilding)
                {
                    return false;
                }
            }

            foreach (string tag in _compareTapStringList)
            {
                if (collider.CompareTag(tag))
                {
                    return false;
                }
            }
        }

        // 2. 불에서부터의 거리 확인
        float distanceFromFire = Vector2.SqrMagnitude(centerOfBuilding);
        if (!FireManager.Instance.IsWithInFireRange(distanceFromFire))
        {
            return false;
        }

        return true;
    }

    private void ConstructBuilding()
    {
        // 새로운 건물 소환
        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)_previewBuilding.BuildingType]);
        newBuilding.transform.position = _previewBuilding.transform.position;

        AudioManager.Instance.PlayBuildAudio();
        CameraManager.Instance.CameraShake(0.1f, 0.3f);

        // 새로운 건물 설정
        ABaseBuilding building = newBuilding.GetComponent<ABaseBuilding>();
        if (building == null)
        {
            return;
        }

        // 건물 초기화
        building.Initialize(_buildDataDic[building.BuildingType]);

        if (building.BuildingType == BuildingType.Forge)
        {
            Forge forge = (Forge)building;
            forge.SetButton();
            forge.OpenSelectToolTypeUI();
        }
        
        // 건물 관리용 우선순위 큐에 추가
        float distanceFromFire = Vector2.SqrMagnitude((Vector2)_previewBuilding.transform.position + _previewBuildingCollider.offset);
        _buildingPriorityQueue.Enqueue(building, -distanceFromFire);

        // 건물 딕셔너리 리스트에 값 추가
        _buildingDicList[building.BuildingType].Add(building);

        OnChangeBuildingList.Invoke(building);

        if (building.BuildingType == BuildingType.Laboratory)
        {
            _isLaboratoryBuilded = true;
        }

        // 기존 프리뷰 건물 제거
        EndBuildingMode();
    }

    public void StartBuildingMode(int buildingTypeWithTool)
    {
        if (buildingTypeWithTool > (int)BuildingType.Forge)
        {
            Debug.Log(buildingTypeWithTool);
            Debug.Log("[박우영]BuildingType 범위 밖입니다. 실행을 종료하고, 버튼의 OnClick() 메서드를 잘 확인하세요");
            return;
        }

        if (_previewBuilding != null)
        {
            return;
        }

        if (!IsCompleteSelectForgeToolType)
        {
            Debug.Log("[박우영] 아직 대장간의 타입을 정해주지 않았습니다.");
            UIManager.Instance.SetCanBuildStart(false);
            return;
        }

        BuildingType buildingType = (BuildingType)buildingTypeWithTool;

        if (buildingType == BuildingType.Laboratory && _isLaboratoryBuilded)
        {
            Debug.Log("[박우영]연구소는 한 개만 생성 가능합니다!");

            // TODO : 팝업 창으로 안내 메세지
            UIManager.Instance.SetCanBuildStart(false);

            return;
        }

        // 자원 소모 체크
        bool canBuild = true;
        canBuild = InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Wood, _requiredResourcesDicList[buildingType][0]);
        if (!canBuild)
        {
            Debug.Log("나무 자원 부족");
            UIManager.Instance.SetCanBuildStart(false);
            return;
        }
        canBuild = InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Stone, _requiredResourcesDicList[buildingType][1]);
        if (!canBuild)
        {
            Debug.Log("돌 자원 부족");
            UIManager.Instance.SetCanBuildStart(false);
            return;
        }

        IsCreateMode = true;

        // UI 건물 설정
        UIManager.Instance.SetCanBuildStart(true);
        GameObject newBuilding = Instantiate(BuildingPrefabs[buildingTypeWithTool]);

        // 프리뷰 건물
        _previewBuilding = newBuilding.GetComponent<ABaseBuilding>();
        _previewBuildingCollider = newBuilding.GetComponent<BoxCollider2D>();

        // 프리뷰 건물 설정
        newBuilding.tag = "Untagged";
        _previewBuilding.SetNavMeshObstacle(false);
    }

    public void EndBuildingMode()
    {
        Destroy(_previewBuilding.gameObject);
        _previewBuilding = null;
        _previewBuildingCollider = null;

        StartCoroutine("StopCreateMode");
    }

    private IEnumerator StopCreateMode()
    {
        yield return new WaitForSeconds(0.1f);
        IsCreateMode = false;
    }

    // 업데이트 전 반지름
    private float _lastUpdatedRange = 0f;
    public void UpdateFireRange()
    {
        // 범위가 더 커진 경우=> 비활성화 -> 활성화
        if (_lastUpdatedRange < FireManager.Instance.GetRange())
        {
            // 비활성화 스택 검사
            while (_disabledBuildingStack.Count != 0)
            {
                // 스택에서 장 상단에 있는 건물 받아오기
                (ABaseBuilding, float) building = _disabledBuildingStack.Peek();

                // 거리의 측정
                if (!FireManager.Instance.IsWithInFireRange(-building.Item2))
                {
                    break;      // 제일 상단의 건물이 범위 밖이면 바로 while 문 종료
                }

                // 반지름 안쪽에 있는 경우 다시 활성화
                _disabledBuildingStack.Pop();
                _buildingPriorityQueue.Enqueue(building.Item1, building.Item2);
                building.Item1.ChangeColor(CanBuildColor);

                building.Item1.SetActive(true);

                OnChangeBuildingActive.Invoke(building.Item1);
            }
        }
        else
        {
            // 범위가 작아진 경우 => 활성화 -> 비활성화
            // 활성화 우선순위 큐 검사
            while (_buildingPriorityQueue.Count != 0)
            {
                // 거리까지 한 번에 받아오기
                (ABaseBuilding, float) building = _buildingPriorityQueue.Dequeue();

                // 반지름 안쪽이면 멈추기
                if (FireManager.Instance.IsWithInFireRange(-building.Item2))
                {
                    _buildingPriorityQueue.Enqueue(building.Item1, building.Item2);
                    break;      // 제일 상단의 건물이 뻠위 안쪽이면 while 문 종료
                }

                // 반지름 바깥쪽이면 비활성화
                _disabledBuildingStack.Push(building);

                building.Item1.ChangeColor(CannotBuildColor);
                
                building.Item1.SetActive(false);

                OnChangeBuildingActive.Invoke(building.Item1);
            }
        }

        _lastUpdatedRange = FireManager.Instance.GetRange();
    }

    public Dictionary<BuildingType, List<ABaseBuilding>> GetBuildingDictionary()
    {
        return _buildingDicList;
    }

    public void UpgradeBuilding(ABaseBuilding building)
    {
        OnChangeBuildingList.Invoke(building);
    }
}
