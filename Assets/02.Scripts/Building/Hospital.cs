using UnityEngine;

public class Hospital : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Hospital;

    private int _medicineCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _medicineCapacity = buildData.Upgrade_AddValueList[0];

        Debug.Log($"Medicine : {_medicineCapacity}");
    }

    public override void UpgradeBuilding()
    {
        base.UpgradeBuilding();

        if (Level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            Debug.Log($"Max Upgrade");
            return;
        }
        _medicineCapacity = _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"Medicine : {_medicineCapacity}");
    }
}