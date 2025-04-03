using UnityEngine;
using System.Collections.Generic;

public class ToolManager : BehaviourSingleton<ToolManager>
{
    private List<ATool> ToolList;
    private List<int> CurrentToolCountList;
    private List<int> MaxToolCountList;

    public readonly int MaxUpgradeLevel = 4;
    public readonly int StartTid = 10000;

    private void Awake()
    {
       // 리스트들 초기화
    }

    public int GetCurrentToolCount(ToolType toolType)
    {
        return CurrentToolCountList[(int)toolType];
    }

    public ATool GetTool(ToolType toolType)
    {
        return ToolList[(int)toolType];
    }

    public void UpgradeTool(ToolType toolType)
    {
        ATool Tool = GetTool(toolType);

        if (Tool.UpgradeLevel >= ToolManager.Instance.MaxUpgradeLevel) return;
        // 스탯 늘리기
        Tool.UpgradeLevel++;
        Tool.Value += DataTable.Instance.GetToolData(StartTid + (int)toolType).Upgrade_ValueList[Tool.UpgradeLevel];
    }

    public void AddCurrentToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        if (CurrentToolCountList[(int)toolType] + amount > MaxToolCountList[(int)toolType]) CurrentToolCountList[(int)toolType] = MaxToolCountList[(int)toolType];
        else CurrentToolCountList[(int)toolType] += amount;
    }

    public void RemoveCurrentToolCount(ToolType toolType, int amount)
    {
        if(amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        if (CurrentToolCountList[(int)toolType] < amount) CurrentToolCountList[(int)toolType] = 0;
        else CurrentToolCountList[(int)toolType] -= amount;
    }

    public void AddMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        MaxToolCountList[(int)toolType] += amount;
    }

    public void RemoveMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        MaxToolCountList[(int)toolType] -= amount;
    }
}
