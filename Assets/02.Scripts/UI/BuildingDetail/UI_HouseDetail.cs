using TMPro;
using UnityEngine;

public class UI_HouseDetail : UI_BuildingData
{
    [Header("유닛 수 텍스트")]
    public TextMeshProUGUI UnitCountText;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        UnitCountText.text = $"{UnitManager.Instance.UnitList.Count} / {UnitManager.Instance.MaxCountUnit}";    
    }

    protected override void RefreshUI()
    {
        base.RefreshUI();

        UnitCountText.text = $"{UnitManager.Instance.UnitList.Count} / {UnitManager.Instance.MaxCountUnit}";
    }
}
