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

    [Header("유닛 디테일 팝업")]
    public UI_UnitData UnitDetail;

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

        ToolListPopup.GetComponent<UI_UnitList>().Initialize();
    }

    private void Update()
    {
        if (_currentOpenUI == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePopUpUI(null);
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

        BuildingListPopup.SetActive(false);
    }

    public void OnClickFire()
    {
        FireObject.SetActive(true);
    }

    public void OpenBuildingDetail(ABaseBuilding building)
    {
        if (building.BuildingType == BuildingType.Laboratory)
        {
            //UILaboratory.gameObject.SetActive(true);
            ChangePopUpUI(UILaboratory.gameObject);
            UILaboratory.Initialize(building);
            return;
        }

        if (building.BuildingType == BuildingType.Forge)
        {
            // UIBuildingList[(int)BuildingType.Forge - 1].gameObject.SetActive(true);
            ChangePopUpUI(UIBuildingList[(int)BuildingType.Forge - 1].gameObject);
            UIBuildingList[(int)BuildingType.Forge - 1].Initialize(building);
            return;
        }

        ChangePopUpUI(UIBuildingList[(int)building.BuildingType].gameObject);
        UIBuildingList[(int)building.BuildingType].Initialize(building);
    }

    public void ChangePopUpUI(GameObject newPopUpUI)
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
    }

    public void OpenUnitDetail(Unit unit)
    {
        UnitDetail.Initialize(unit);
        UnitDetail.transform.parent.gameObject.SetActive(true);
    }
}
