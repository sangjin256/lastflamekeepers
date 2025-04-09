using UnityEngine;
using System.Collections.Generic;

public class ToolManager : BehaviourSingleton<ToolManager>
{
    private List<ATool> _toolList;
    private List<int> _currentToolCountList;
    private List<int> _maxToolCountList;

    public readonly int MaxUpgradeLevel = 3;
    public readonly int StartTid = 10000;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += _LoadTool;
    }

    private void _LoadTool()
    {
        _toolList = new List<ATool>();
        _maxToolCountList = new List<int>();

        ToolData data = DataTable.Instance.GetToolData(10000);
        _toolList.Add(new SwordTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10001);
        _toolList.Add(new AxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10002);
        _toolList.Add(new PickAxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10003);
        _toolList.Add(new MedicineTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        _currentToolCountList = new List<int>(_toolList.Count);

        Debug.Log("Tool Data Loaded");
    }

    public int GetCurrentToolCount(ToolType toolType)
    {
        return _currentToolCountList[(int)toolType];
    }

    public ATool GetTool(ToolType toolType)
    {
        return _toolList[(int)toolType];
    }

    public void UpgradeTool(ToolType toolType)
    {
        ATool Tool = GetTool(toolType);

        if (Tool.UpgradeLevel >= ToolManager.Instance.MaxUpgradeLevel) return;
        // 스탯 늘리기
        Tool.UpgradeLevel++;
        Tool.Value += DataTable.Instance.GetToolData(StartTid + (int)toolType).Upgrade_AddValueList[Tool.UpgradeLevel];
    }

    public void AddCurrentToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        if (_currentToolCountList[(int)toolType] + amount > _maxToolCountList[(int)toolType]) _currentToolCountList[(int)toolType] = _maxToolCountList[(int)toolType];
        else _currentToolCountList[(int)toolType] += amount;
    }

    public void RemoveCurrentToolCount(ToolType toolType, int amount)
    {
        if(amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        if (_currentToolCountList[(int)toolType] < amount) _currentToolCountList[(int)toolType] = 0;
        else _currentToolCountList[(int)toolType] -= amount;
    }

    public void AddMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        _maxToolCountList[(int)toolType] += amount;
    }

    public void RemoveMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        _maxToolCountList[(int)toolType] -= amount;
    }
}
