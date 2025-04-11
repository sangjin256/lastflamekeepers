using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using System.Linq;

public class ToolManager : BehaviourSingleton<ToolManager>
{
    private List<ATool> _toolList;
    private List<int> _currentToolCountList;
    private List<int> _maxToolCountList;

    public readonly int MaxUpgradeLevel = 3;
    public readonly int StartTid = 10000;

    public Action<Unit> OnToolChanged;

    public Action OnToolCountChanged;

    private AnimationClip[,,] _toolAnimationClip = new AnimationClip[5, 4, 2];
    public AnimationClip[,,] ToolAnimationClip => _toolAnimationClip;

    public AnimationClip[] NoneAnimatinoClip;
    public AnimationClip[] SwordAnimationClip;
    public AnimationClip[] AxeAnimationClip;
    public AnimationClip[] PickaxeAnimationClip;
    public AnimationClip[] CrossAnimationClip;


    private AnimationClip[][] _animationClipArray;

    public ATool HandOverTool;

    public LayerMask Clilckable;

    public GameObject[] ToolImagePrefab;

    public GameObject handOverToolImage;


    public Sprite[] NoneImage;
    public Sprite[] SwordImage;
    public Sprite[] AxeImage;
    public Sprite[] PickaxeImage;
    public Sprite[] CrossImage;
    private Sprite[][] _imageArray;
    private List<ToolData> _toolDataList = new List<ToolData>();
    private void Awake()
    {
        Global.Instance.OnDataLoaded += _LoadTool;
        InitializeAnimationClip();
        InitializeImage();
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

        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastHit2D> hitList = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, Clilckable).ToList();
            if (hitList.Count != 0)
            {
                RaycastHit2D unitObject = hitList.Find(x => x.collider.transform.parent.CompareTag("Unit"));
                if (unitObject)
                {
                    Unit unit = unitObject.transform.GetComponent<Unit>();
                    TrySetTool(unit, HandOverTool.ToolType);
                    EndHandOverToolMode();
                    return;
                }
                else
                {
                    EndHandOverToolMode();
                    return;
                }
            }
            EndHandOverToolMode();
            return;
        }
    }
    private void _LoadTool()
    {
        _toolList = new List<ATool>();
        _maxToolCountList = new List<int>();

        //맨손
        ToolData data = DataTable.Instance.GetToolData(10000);
        _toolList.Add(new NoneTool(data.ToolType, data.ToolName, 0, 0));
        _toolDataList.Add(data);
        _maxToolCountList.Add(999);

        data = DataTable.Instance.GetToolData(10001);
        _toolList.Add(new SwordTool(data.ToolType, data.ToolName, 0, data.Value));
        _toolDataList.Add(data);
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10002);
        _toolList.Add(new AxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _toolDataList.Add(data);
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10003);
        _toolList.Add(new PickAxeTool(data.ToolType, data.ToolName, 0, data.Value));
        _toolDataList.Add(data);
        _maxToolCountList.Add(data.MaxCount);

        data = DataTable.Instance.GetToolData(10004);
        _toolList.Add(new MedicineTool(data.ToolType, data.ToolName, 0, data.Value));
        _toolDataList.Add(data);
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
        Debug.Log($"{tooltype} : {GetCurrentToolCount(tooltype)} / {GetMaxToolCount(tooltype)}");
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

    public bool CheckCanUpgrade(ToolType type)
    {
        return GetTool(type).UpgradeLevel < ToolManager.Instance.MaxUpgradeLevel;
    }

    public int GetWoodCountToUpgrade(ToolType type)
    {
        if (CheckCanUpgrade(type) == false) return -1;
        return _toolDataList[(int)type].Upgrade_WoodCountList[_toolList[(int)type].UpgradeLevel];
    }

    public int GetStoneCountToUpgrade(ToolType type)
    {
        if (CheckCanUpgrade(type) == false) return -1;
        return _toolDataList[(int)type].Upgrade_StoneCountList[_toolList[(int)type].UpgradeLevel];
    }

    public int GetCurrentLevel(ToolType type)
    {
        return GetTool(type).UpgradeLevel + 1;
    }

    public void UpgradeTool(ToolType toolType)
    {
        ATool Tool = GetTool(toolType);

        if (CheckCanUpgrade(toolType) == false) return;
        // 스탯 늘리기
        Tool.Value += _toolDataList[(int)toolType].Upgrade_AddValueList[Tool.UpgradeLevel];


        Tool.UpgradeLevel++;
        foreach (Unit unit in UnitManager.Instance.UnitList)
        {
            if (unit.UnitTool.CurrentTool == Tool)
            {
                TrySetTool(unit, toolType);
            }
        }
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
        _animationClipArray = new AnimationClip[][] { NoneAnimatinoClip, SwordAnimationClip, AxeAnimationClip, PickaxeAnimationClip, CrossAnimationClip };

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

    public void InitializeImage()
    {
        _imageArray = new Sprite[][] { NoneImage, SwordImage, AxeImage, PickaxeImage, CrossImage };
    }

    public void StartHandOverToolMode(int toolType)
    {
        if (toolType > (int)ToolType.Medicine)
        {
            Debug.Log("[심형준]핸드오버툴 오류");
            return;
        }

        if (HandOverTool != null)
        {
            return;
        }

        bool canHandOver = false;
        //툴 사용 가능 개수 검사
        canHandOver = ToolManager.Instance.CheckCanTool((ToolType)toolType);

        if (!canHandOver)
        {
            Debug.Log("도구 수 부족");
            return;
        }


        //도구 이미지 오브젝트 생성
        HandOverTool = GetTool((ToolType)toolType);
        handOverToolImage = Instantiate(ToolImagePrefab[toolType]);

        handOverToolImage.GetComponent<SpriteRenderer>().sprite = _imageArray[toolType][HandOverTool.UpgradeLevel]; 

    }
    public void EndHandOverToolMode()
    {
        HandOverTool = null;
        Destroy(handOverToolImage);
    }

    private void MoveToolToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //도구 이미지 오브젝트
        handOverToolImage.transform.position = mousePosition;
    }
}
