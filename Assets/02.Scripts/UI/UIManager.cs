using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BehaviourSingleton<UIManager>
{
    [Header("°Ç¹° »ý¼º ¹öÆ°")]
    public List<Button> BuildingManageButtons;
    public GameObject BuildingListPopup;

    [Header("À¯´Ö Åø ¹öÆ°")]
    public List<Button> UnitToolManageButtons;
    public GameObject ToolListPopup;

    private bool _canBuildStart;
    public GameObject FireObject;

    [Header("°Ç¹° µðÅ×ÀÏ ÆË¾÷")]
    public List<UI_BuildingData> UIBuildingList;
    public UI_LaboratoryDetail UILaboratory;
    private GameObject _currentOpenUI = null;


    [Header("À¯´Ö µðÅ×ÀÏ ÆË¾÷")]
    public UI_UnitData UnitDetail;

    [Header("ÆË¾÷ À§Ä¡")]
    public Vector2 BasicPopUpPosition = new Vector2(530, - 110);
    public Vector2 BulidingListPopUpPosition = new Vector2(-235, -210);
    private Vector2 _centerPivot = new Vector2(0.5f, 0.5f);
    private Vector2 _middleLeftPivot = new Vector2(0, 0.5f);

    private GameObject _clickedObject = null;

    public List<Vector2> PopUpPositionList;


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

        ChangePopUpUI(null);

        BuildingListPopup.SetActive(false);
    }

    public void OnClickFire()
    {
        FireObject.SetActive(true);
    }

    public void OpenBuildingDetail(ABaseBuilding building, bool isUseBuildingList)
    {
        int detailPositionIndex = isUseBuildingList ? 1 : 0;

        if (isUseBuildingList)
        {
            TryMoveCameraToObject(building.gameObject);
        }
        

        if (building.BuildingType == BuildingType.Laboratory)
        {
            ChangePopUpUI(UILaboratory.gameObject, detailPositionIndex);
            UILaboratory.Initialize(building);
            return;
        }

        if (building.BuildingType == BuildingType.Forge)
        {
            if (!BuildManager.Instance.IsCompleteSelectForgeToolType)
            {
                ChangePopUpUI(null);
                return;
            }
            ChangePopUpUI(UIBuildingList[(int)BuildingType.Forge - 1].gameObject, detailPositionIndex);
            UIBuildingList[(int)BuildingType.Forge - 1].Initialize(building);
            return;
        }

        ChangePopUpUI(UIBuildingList[(int)building.BuildingType].gameObject, detailPositionIndex);
        UIBuildingList[(int)building.BuildingType].Initialize(building);
    }

    public void ChangePopUpUI(GameObject newPopUpUI, int popUpPositionListIndex)
    {
        if (popUpPositionListIndex >= PopUpPositionList.Count)
        {
            Debug.Log("¿À¹ö·¦ ÀÎµ¦½º");
            return;
        }

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

        if (popUpPositionListIndex == 0)
        {
            _currentOpenUI.GetComponent<RectTransform>().pivot = _centerPivot;
        }
        else
        {
            _currentOpenUI.GetComponent<RectTransform>().pivot = _middleLeftPivot;
        }

        _currentOpenUI.transform.localPosition = PopUpPositionList[popUpPositionListIndex];
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

        if (_currentOpenUI == null)
        {
            return;
        }
    }

    

    public void OpenUnitDetail(Unit unit, bool isUseUnitList)
    {
        UnitDetail.Initialize(unit);
        UnitDetail.transform.parent.gameObject.SetActive(true);

        if (isUseUnitList)
        {
            TryMoveCameraToObject(unit.gameObject);
            ChangePopUpUI(UnitDetail.gameObject, 2);
            return;
        }
        ChangePopUpUI(UnitDetail.gameObject, 0);
    }

    private void TryMoveCameraToObject(GameObject clickedObject)
    {
        if (_clickedObject != clickedObject)
        {
            _clickedObject = clickedObject;
            return;
        }

        CameraManager.Instance.MoveToEntity(clickedObject.transform);
    }
}
