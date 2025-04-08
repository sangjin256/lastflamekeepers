using System.Collections.Generic;
using UnityEngine;

public class Hospital : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Hospital;

    private int _medicineCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _medicineCapacity = buildData.Value;

        // ToolManager의 Tool 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(ToolType.Medicine, _medicineCapacity);

        Debug.Log($"Medicine : {_medicineCapacity}");
    }

    public override bool UpgradeBuilding()
    {
        bool upgradeSuccess = base.UpgradeBuilding();

        if (!upgradeSuccess)
        {
            return false;
        }

        // ToolManager의 Medicine 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(ToolType.Medicine, _buildData.Upgrade_AddValueList[Level]);
        _medicineCapacity += _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"Medicine : {_medicineCapacity}");

        return true;
    }

    public override void SetActive(bool active)
    {
        // 이전 상태와 같다면 return
        if (_isActive == active)
        {
            return;
        }

        // 활성화 여부에 따른 전체 Medicine 개수 변화
        if (active)
        {
            ToolManager.Instance.AddMaxToolCount(ToolType.Medicine, _medicineCapacity);
        }
        else
        {
            ToolManager.Instance.RemoveMaxToolCount(ToolType.Medicine, _medicineCapacity);
        }

        _isActive = active;
    }
}