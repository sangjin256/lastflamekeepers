using System.Collections.Generic;
using UnityEngine;

public class House : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.House;

    private int _unitCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _unitCapacity = buildData.Value;
        // Unit 최댓값 증가
        UnitManager.Instance.AddMaxCountUnit(_unitCapacity);

        Debug.Log($"Unit : {_unitCapacity}");
    }

    public override bool UpgradeBuilding()
    {
        bool upgradeSuccess = base.UpgradeBuilding();

        if (!upgradeSuccess)
        {
            return false;
        }

        // Unit 최댓값 증가
        UnitManager.Instance.AddMaxCountUnit(_buildData.Upgrade_AddValueList[Level - 1]);
        _unitCapacity += _buildData.Upgrade_AddValueList[Level - 1];

        Debug.Log($"Unit : {_unitCapacity}");

        return true;
    }

    public override void SetActive(bool active)
    {
        // 이전 상태와 같다면 return
        if (_isActive == active)
        {
            return;
        }

        // 활성화 여부에 따른 전체 Unit 수 변화
        if (active)
        {
            UnitManager.Instance.AddMaxCountUnit(_unitCapacity);
        }
        else
        {
            UnitManager.Instance.RemoveMaxCountUnit(_unitCapacity);
        }

        _isActive = active;
    }
}
