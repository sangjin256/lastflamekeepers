using System.Collections.Generic;
using UnityEngine;

public class Storage : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Storage;

    private int _resourceCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _resourceCapacity = buildData.Upgrade_AddValueList[0];

        AddInventoryResourceCapacity(_resourceCapacity);
        Debug.Log($"Max Resource Count : {_resourceCapacity}");
    }

    public override void UpgradeBuilding()
    {
        base.UpgradeBuilding();

        if (Level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            Debug.Log($"Max Upgrade");
            return;
        }

        // 모든 최대 자원 수 변경
        AddInventoryResourceCapacity(_buildData.Upgrade_AddValueList[Level]);

        _resourceCapacity += _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"Max Resource Count : {_resourceCapacity}");
    }

    public override void SetActive(bool active)
    {
        // 이전 상태와 같다면 return
        if (_isActive == active)
        {
            return;
        }

        // 모든 최대 자원 수 변경
        if (active)
        {
            AddInventoryResourceCapacity(_resourceCapacity);
        }
        else
        {
            RemoveInventoryResourceCapacity(_resourceCapacity);
        }

        _isActive = active;
    }

    private void AddInventoryResourceCapacity(int amount)
    {
        for (int i = 0; i < (int)InventoryResourceType.Ash; i++)
        {
            InventoryResourceManager.Instance.TryAddMaxResourceCount((InventoryResourceType)i, amount);
        }
    }

    private void RemoveInventoryResourceCapacity(int amount)
    {
        for (int i = 0; i < (int)InventoryResourceType.Ash; i++)
        {
            InventoryResourceManager.Instance.TryRemoveMaxResourceCount((InventoryResourceType)i, amount);
        }
    }
}
