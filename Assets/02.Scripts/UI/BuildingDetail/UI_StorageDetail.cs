using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StorageDetail : UI_BuildingData
{
    public List<TextMeshProUGUI> ResourceAmountTextList;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        for (int i = 0; i < ResourceAmountTextList.Count; i++)
        {
            ResourceAmountTextList[i].text 
                = $"{InventoryResourceManager.Instance.GetCurrentResourceCount((InventoryResourceType)i)} / {InventoryResourceManager.Instance.GetMaxResourceCount((InventoryResourceType)i)}";
        }
    }

    protected override void RefreshUI()
    {
        base.RefreshUI();

        for (int i = 0; i < ResourceAmountTextList.Count; i++)
        {
            ResourceAmountTextList[i].text
                = $"{InventoryResourceManager.Instance.GetCurrentResourceCount((InventoryResourceType)i)} / {InventoryResourceManager.Instance.GetMaxResourceCount((InventoryResourceType)i)}";
        }
    }
}
