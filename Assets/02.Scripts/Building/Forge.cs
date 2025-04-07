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
        
        _toolCapacity = buildData.Upgrade_AddValueList[0];
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

    public void SetButton()
    {
        _buttons = GetComponentsInChildren<Button>(true);
        Debug.Log(_buttons.Length);
    }

    public void OpenSelectToolTypeUI()
    {

        int a = 3;
        foreach (Button button in _buttons)
        {
            button.gameObject.SetActive(true);
        }
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

        Debug.Log($"{_toolType} : {_toolCapacity}");

        CloseSelectToolTypeUI();
    }
}