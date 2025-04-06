using UnityEngine;

public class House : BaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.House;

    private int _unitCapacity;

    public override void SetBuildData(BuildData buildData)
    {
        base.SetBuildData(buildData);

        _unitCapacity = buildData.Upgrade_AddValueList[0];

        Debug.Log($"Unit : {_unitCapacity}");
    }

    public override void UpgradeBuilding()
    {
        base.UpgradeBuilding();

        if (Level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            Debug.Log($"Unit : {_unitCapacity}");
            return;
        }
        _unitCapacity = _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"Unit : {_unitCapacity}");
    }
}
