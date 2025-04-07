using System.Collections.Generic;
using UnityEngine;

public class BuildManager : BehaviourSingleton<BuildManager>
{
    [Header("빌딩 프리팹")]
    public List<GameObject> BuildingPrefabs;        // 배치할 건물 프립팹
    private ABaseBuilding _previewBuilding = null;       // 프리뷰용 건물

    [Header("프리뷰 건물 색")]
    public Color CanBuildColor = Color.white;
    public Color CannotBuildColor = Color.red;


    // 소환된 건물들
    private PriorityQueue<ABaseBuilding, float> _buildingPriorityQueue;          // 활성화 된 건물 관리 우선순위 큐
    private Stack<(ABaseBuilding, float)> _disabledBuildingStack;                // 비활성화 된 건물 관리 스택
    private Dictionary<BuildingType, List<int>> _buildingCountDicList;      // 건물 타입별 레벨별 개수
    
    // 건물별 데이터 리스트
    private ReadOnlyList<BuildData> _buildDataList;

    private void Start()
    {
        // 데이터 받아오기
        Global.Instance.OnDataLoaded += _LoadBuildData;

        // FireManager 범위 변경 이벤트 구독
        FireManager.Instance.OnFireRangeChanged += UpdateFireRange;
    }


    public void _LoadBuildData()
    {
        _buildDataList = DataTable.Instance.GetBuildDataList();

        Initialize();
    }

    private void Initialize()
    {
        // 우선순위 큐 초기화
        _buildingPriorityQueue = new PriorityQueue<ABaseBuilding, float>();

        // 딕셔너리 리스트 초기화
        _buildingCountDicList = new Dictionary<BuildingType, List<int>>();
        // TODO : 건물 추가 시 BuildingType.Count 추가 및 수정
        for (int i = 0; i <= (int)BuildingType.Forge; i++)
        {
            // 레벨의 크기만큼 리스트 초기화
            int upgradeCount = _buildDataList[0].Upgrade_AddValueList.Count;
            List<int> levelCount = new List<int>(upgradeCount);

            for (int j = 0; j < upgradeCount; j++)
            {
                levelCount.Add(0);
            }

            // 건물 타입별 딕셔너리 리스트 추가
            _buildingCountDicList.Add((BuildingType)i, levelCount);
        }

        // 스택 초기화
        _disabledBuildingStack = new Stack<(ABaseBuilding, float)>();
    }

    private void Update()
    {
        // 현재 건물 짓는 모드인지 확인
        if (_previewBuilding == null)
        {
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

        _previewBuilding.transform.position = mousePosition;
    }

    private bool CheckBuildCondition()
    {
        // 1. 다른 건물과 충돌 여부 확인
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_previewBuilding.transform.position, _previewBuilding.GetComponent<BoxCollider2D>().size, 0);
        foreach (Collider2D collider in colliders)
        {
            ABaseBuilding building = collider.GetComponent<ABaseBuilding>();
            if (building && building != _previewBuilding)
            {
                return false;
            }
        }

        // 2. 불에서부터의 거리 확인
        float distanceFromFire = Vector2.SqrMagnitude(_previewBuilding.transform.position);
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

        // 새로운 건물 설정
        ABaseBuilding building = newBuilding.GetComponent<ABaseBuilding>();
        if (building == null)
        {
            return;
        }

        building.Initialize(_buildDataList[(int)building.BuildingType]);

        // 건물 관리용 우선순위 큐에 추가
        float distanceFromFire = Vector2.SqrMagnitude(_previewBuilding.transform.position);
        _buildingPriorityQueue.Enqueue(building, -distanceFromFire);

        // 건물 개수 리스트에 값 추가
        _buildingCountDicList[building.BuildingType][0] += 1;

        // 기존 프리뷰 건물 제거
        EndBuildingMode();
    }

    public void StartBuildingMode(BuildingType buildingType)
    {
        if (_previewBuilding != null)
        {
            return;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)buildingType]);
        _previewBuilding = newBuilding.GetComponent<ABaseBuilding>();
    }

    public void StartBuildingHouse()
    {
        if (_previewBuilding != null)
        {
            return;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)BuildingType.House]);
        _previewBuilding = newBuilding.GetComponent<ABaseBuilding>();
    }

    public void EndBuildingMode()
    {
        Destroy(_previewBuilding.gameObject);
        _previewBuilding = null;
    }

    // 업데이트 전 반지름
    private float _lastUpdatedRange = 0f;
    public void UpdateFireRange(float newRange)
    {
        // 범위가 더 커진 경우=> 비활성화 -> 활성화
        if (_lastUpdatedRange < newRange)
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

                _buildingCountDicList[building.Item1.BuildingType][building.Item1.Level] += 1;
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
                _buildingCountDicList[building.Item1.BuildingType][building.Item1.Level] -= 1;

            }
        }

        _lastUpdatedRange = newRange;
    }

    public int GetBuildingCount(BuildingType type, int level)
    {
        return _buildingCountDicList[type][level];
    }
}
