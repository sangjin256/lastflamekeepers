using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BehaviourSingleton<UIManager>
{
    [Header("건물 생성 버튼")]
    public List<Button> BuildingManageButtons;
    public GameObject BuildingListPopup;

    [Header("유닛 툴 버튼")]
    public List<Button> UnitToolManageButtons;
    public GameObject ToolListPopup;

    private bool _canBuildStart;
    public GameObject FireObject;

    [Header("건물 디테일 팝업")]
    public List<UI_BuildingData> UIBuildingList;
    public UI_LaboratoryDetail UILaboratory;
    private GameObject _currentOpenUI = null;

    [Header("팝업 위치")]
    public Vector2 BasicPopUpPosition = new Vector2(530, - 110);
    public Vector2 BulidingListPopUpPosition = new Vector2(-235, -210);
    private Vector2 _centerPivot = new Vector2(0.5f, 0.5f);
    private Vector2 _middleLeftPivot = new Vector2(0, 0.5f);

    public void SetCanBuildStart(bool canBuildStart)
    {
        _canBuildStart = canBuildStart;
    }

    private void Start()
    {
        for (int i = 0; i <= (int)BuildingType.Forge; i++)
        {
            int index = i;
            BuildingManageButtons[i].onClick.AddListener(() => BuildManager.Instance.StartBuildingMode(index));
            BuildingManageButtons[i].onClick.AddListener(TryCloseBuildingListPopup);
        }

        _canBuildStart = false;


        for (int i = 0; i <= (int)ToolType.Medicine; i++)
        {
            int index = i;
            UnitToolManageButtons[i].onClick.AddListener(() => ToolManager.Instance.StartHandOverToolMode(index));
            UnitToolManageButtons[i].onClick.AddListener(TryCloseToolListPopup);
        }
    }

    private void Update()
    {
        if (_currentOpenUI == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePopUpUI(null, false);
        }
    }

    public void TryCloseToolListPopup()
    {
        ToolListPopup.SetActive(false);
    }

    public void TryCloseBuildingListPopup()
    {
        if (!_canBuildStart)
        {
            return;
        }

        ChangePopUpUI(null, false);

        BuildingListPopup.SetActive(false);
    }

    public void OnClickFire()
    {
        FireObject.SetActive(true);
    }

    public void OpenBuildingDetail(ABaseBuilding building, bool isUseBuildingList)
    {
        if (building.BuildingType == BuildingType.Laboratory)
        {
            ChangePopUpUI(UILaboratory.gameObject, isUseBuildingList);
            UILaboratory.Initialize(building);
            return;
        }

        if (building.BuildingType == BuildingType.Forge)
        {
            ChangePopUpUI(UIBuildingList[(int)BuildingType.Forge - 1].gameObject, isUseBuildingList);
            UIBuildingList[(int)BuildingType.Forge - 1].Initialize(building);
            return;
        }

        ChangePopUpUI(UIBuildingList[(int)building.BuildingType].gameObject, isUseBuildingList);
        UIBuildingList[(int)building.BuildingType].Initialize(building);
    }

    public void ChangePopUpUI(GameObject newPopUpUI, bool isUseBuildingList)
    {
        if (_currentOpenUI != null)
        {
            _currentOpenUI.SetActive(false);
        }

        if (newPopUpUI != null)
        {
            newPopUpUI.SetActive(true);
        }

        _currentOpenUI = newPopUpUI;

        if (_currentOpenUI == null)
        {
            return;
        }

        if (isUseBuildingList)
        {
            _currentOpenUI.GetComponent<RectTransform>().pivot = _middleLeftPivot;
            _currentOpenUI.transform.localPosition = BulidingListPopUpPosition;
        }
        else
        {
            _currentOpenUI.GetComponent<RectTransform>().pivot = _centerPivot;
            _currentOpenUI.transform.localPosition = BasicPopUpPosition;
        }
    }
}
