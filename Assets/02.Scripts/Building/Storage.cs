using UnityEngine;

public class Storage : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Storage;

    private int _toolCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _toolCapacity = buildData.Upgrade_AddValueList[0];

        Debug.Log($"Unit : {_toolCapacity}");
    }

    public override void UpgradeBuilding()
    {
        base.UpgradeBuilding();

        if (Level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            Debug.Log($"Unit : {_toolCapacity}");
            return;
        }
        _toolCapacity = _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"Unit : {_toolCapacity}");
    }
}
