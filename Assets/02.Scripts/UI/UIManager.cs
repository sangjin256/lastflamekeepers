using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("건물 생성 버튼")]
    public List<Button> BuildingManageButtons;

    [Header("유닛 툴 버튼")]
    public List<Button> UnitToolManageButtons;

    private void Start()
    {
        for (int i = 0; i <= (int)BuildingType.Forge; i++)
        {
            int index = i;
            BuildingManageButtons[i].onClick.AddListener(() => BuildManager.Instance.StartBuildingMode(index));
        }
    }
}
