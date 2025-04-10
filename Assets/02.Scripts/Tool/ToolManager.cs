using UnityEngine;
using System.Collections.Generic;
using System;

public class ToolManager : BehaviourSingleton<ToolManager>
{
    private List<ATool> _toolList;
    private List<int> _currentToolCountList;
    private List<int> _maxToolCountList;

    public readonly int MaxUpgradeLevel = 3;
    public readonly int StartTid = 10000;

    public Action<Unit> OnToolChanged;

    public Action OnToolCountChanged;


    private AnimationClip[,,] _toolAnimationClip = new AnimationClip[4, 4, 2];
    public AnimationClip[,,] ToolAnimationClip => _toolAnimationClip;

    public AnimationClip[] NoneAnimatinoClip;
    public AnimationClip[] SwordAnimationClip;
    public AnimationClip[] AxeAnimationClip;
    public AnimationClip[] PickaxeAnimationClip;

    private AnimationClip[][] _animationClipArray;

    public ATool HandOverTool;
    private void Awake()
    {
        Global.Instance.OnDataLoaded += _LoadTool;
        InitializeAnimationClip();
    }
    private void Update()
    {
        if (HandOverTool == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndHandOverToolMode();
            return;
        }
        MoveToolToMouse();
    }
    private void _LoadTool()
    {
        _toolList = new List<ATool>();
        _maxToolCountList = new List<int>();

        //맨손
        ToolData data = DataTable.Instance.GetToolData(10000);
        _toolList.Add(new NoneTool(data.ToolType, data.ToolName, 0, 0));
        _maxToolCountList.Add(999);

        data = DataTable.Instance.GetToolData(10001);
        _toolList.Add(new SwordTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10002);
        _toolList.Add(new AxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10003);
        _toolList.Add(new PickAxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10004);
        _toolList.Add(new MedicineTool(data.ToolType, data.ToolName, 0, data.Value));
        _maxToolCountList.Add(data.MaxCount);

        _currentToolCountList = new List<int>(new int[5]);
        OnToolCountChanged?.Invoke();

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

    public int GetMaxToolCount(ToolType toolType)
    {
        return _maxToolCountList[(int)toolType];
    }

    public bool CheckCanTool(ToolType tooltype)
    {
        return _currentToolCountList[(int)tooltype] < _maxToolCountList[(int)tooltype];
    }

    public bool TrySetTool(Unit unit, ToolType toolType)
    {
        RemoveCurrentToolCount(unit.UnitTool.CurrentTool.ToolType, 1);
        AddCurrentToolCount(toolType, 1);
        unit.SetTool(GetTool(toolType));

        OnToolChanged?.Invoke(unit);
        OnToolCountChanged?.Invoke();

        return true;
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
        OnToolCountChanged?.Invoke();
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
        OnToolCountChanged?.Invoke();
    }

    public void AddMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        _maxToolCountList[(int)toolType] += amount;
        OnToolCountChanged?.Invoke();
    }

    public void RemoveMaxToolCount(ToolType toolType, int amount)
    {
        if (amount < 0)
        {
            Debug.Log("잘못된 수량입니다.");
            return;
        }

        _maxToolCountList[(int)toolType] -= amount;
        OnToolCountChanged?.Invoke();
    }

    public void InitializeAnimationClip()
    {
        _animationClipArray = new AnimationClip[][] { NoneAnimatinoClip, SwordAnimationClip, AxeAnimationClip, PickaxeAnimationClip };

        for (int toolType = 0; toolType < _toolAnimationClip.GetLength(0); toolType++)
        {
            for (int toolLevel = 0; toolLevel < _toolAnimationClip.GetLength(1); toolLevel++)
            {
                for (int state = 0; state < _toolAnimationClip.GetLength(2); state++)
                {
                    _toolAnimationClip[toolType, toolLevel, state] = _animationClipArray[toolType][toolLevel * 2 + state];
                }
            }
        }

    }

    public void StartHandOverToolMode(int toolType)
    {
        if (toolType > (int)ToolType.Medicine)
        {
            Debug.Log("[심형준]핸드오버툴 오류");
            return;
        }

        if (HandOverTool == null)
        {
            return;
        }

        bool canHandOver = false;
        //툴 사용 가능 개수 검사
        //canHandOver = 

        if (!canHandOver)
        {
            Debug.Log("도구 수 부족");
        }

        //도구 이미지 오브젝트 생성
    }
    public void EndHandOverToolMode()
    {
        HandOverTool = null;
    }

    private void MoveToolToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //도구 이미지 오브젝트
        //.transform.position = mousePosition;
    }
}
