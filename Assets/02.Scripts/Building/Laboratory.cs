using System.Collections.Generic;
using UnityEngine;

public class Laboratory : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Laboratory;


    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);


    }

    public void UpgradeTool(int toolTypeNumber)
    {
        if (toolTypeNumber > (int)ToolType.Medicine)
        {
            Debug.Log("[박우영]ToolType enum의 최댓값보다 큰 값입니다. 버튼의 값을 확인해 주세요");
            return;
        }

        ToolType toolType = (ToolType)toolTypeNumber;
        ToolManager.Instance.UpgradeTool(toolType);
    }

    public override void SetActive(bool active)
    {
        // 이전 상태와 같다면 return
        if (_isActive == active)
        {
            return;
        }

        _isActive = active;
    }
}
