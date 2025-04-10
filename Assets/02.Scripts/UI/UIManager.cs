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

    private bool _canBuildStart;
    public GameObject FireObject;

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
}
