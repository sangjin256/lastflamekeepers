using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forge : ABaseBuilding
{
    protected override BuildingType DefineType() => BuildingType.Forge;

    // 업그레이드 할 툴 타입
    ToolType _toolType;
    public ToolType ToolType => _toolType;

    private int _toolCapacity;

    private Button[] _buttons;


    public override void Initialize(BuildData buildData)
    {
        base.Initialize(buildData);

        _toolCapacity = buildData.Value;
    }

    public override bool UpgradeBuilding()
    {
        bool upgradeSuccess = base.UpgradeBuilding();

        if (!upgradeSuccess)
        {
            return false;
        }

        // ToolManager의 Tool 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(_toolType,_buildData.Upgrade_AddValueList[Level - 1]);
        _toolCapacity += _buildData.Upgrade_AddValueList[Level - 1];

        Debug.Log($"{_toolType} : {_toolCapacity}");

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
            ToolManager.Instance.AddMaxToolCount(_toolType, _toolCapacity);
        }
        else
        {
            ToolManager.Instance.RemoveMaxToolCount(_toolType, _toolCapacity);
        }

        _isActive = active;
    }

    public void SetButton()
    {
        _buttons = GetComponentsInChildren<Button>(true);
        Debug.Log(_buttons.Length);
    }

    public void OpenSelectToolTypeUI()
    {
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(true);
        }

        BuildManager.Instance.ChangeIsCompleteSelectForgeToolType(false);
    }

    private void CloseSelectToolTypeUI()
    {
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(false);
        }

    }

    public void SetToolType(int toolType)
    {
        if (toolType > (int)ToolType.PickAxe)
        {
            Debug.Log("[박우영] Button OnClick() 메서드 잘못 입력");
            return;
        }
        _toolType = (ToolType)toolType;

        // ToolManager의 Tool 최댓값 증가
        ToolManager.Instance.AddMaxToolCount(_toolType, _toolCapacity);

        BuildManager.Instance.ChangeIsCompleteSelectForgeToolType(true);

        Debug.Log($"{_toolType} : {_toolCapacity}");

        CloseSelectToolTypeUI();
    }
}