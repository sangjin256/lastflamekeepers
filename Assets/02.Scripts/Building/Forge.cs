using System.Collections.Generic;
using UnityEngine;

public class Forge : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Forge;

    // 업그레이드 할 툴 타입
    ToolType _toolType;
    public ToolType ToolType => _toolType;

    private int _toolCapacity;

    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _toolCapacity = buildData.Upgrade_AddValueList[0];

        // ToolManager의 Tool 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(_toolType, _toolCapacity);

        Debug.Log($"{_toolType} : {_toolCapacity}");
    }

    public override void UpgradeBuilding()
    {
        base.UpgradeBuilding();

        if (Level >= _buildData.Upgrade_AddValueList.Count - 1)
        {
            Debug.Log($"Max Upgrade");
            return;
        }

        // ToolManager의 Tool 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(_toolType,_buildData.Upgrade_AddValueList[Level]);
        _toolCapacity += _buildData.Upgrade_AddValueList[Level];

        Debug.Log($"{_toolType} : {_toolCapacity}");
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
            ToolManager.Instance.AddMaxToolCount(_toolType, _toolCapacity);
        }
        else
        {
            ToolManager.Instance.RemoveMaxToolCount(_toolType, _toolCapacity);
        }

        _isActive = active;
    }

    public void SetToolType(ToolType toolType)
    {
        _toolType = toolType;
    }
}