using System.Collections.Generic;
using UnityEngine;

public class House : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.House;

    private int _unitCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _unitCapacity = buildData.Upgrade_AddValueList[0];
        // TODO : Unit 최댓값 증가

        Debug.Log($"Unit : {_unitCapacity}");
    }

    public override bool UpgradeBuilding()
    {
        bool upgradeSuccess = base.UpgradeBuilding();

        if (!upgradeSuccess)
        {
            return false;
        }

        // TODO : Unit 최댓값 증가
        _unitCapacity += _buildData.Upgrade_AddValueList[Level];

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
            Debug.Log("[박우영]유닛 최대수 증가 함수 필요");
        }
        else
        {
            Debug.Log("[박우영]유닛 최대수 감소 함수 필요");
        }

        _isActive = active;
    }
}
