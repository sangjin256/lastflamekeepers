using FunkyCode;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//  불의 범위를 관리하고 범위가 변경될 때 이벤트를 발생시키는 클래스


public class FireManager : BehaviourSingleton<FireManager>
{
    [Header("불 범위 설정")]
    [SerializeField] int MaxRange = 43;
    [SerializeField] const int MinRange = 8;
    [SerializeField] int CurrentRange = 8;

    private FunkyCode.Light2D _fireLight;

    private const int StartTID = 10000;
    private int WoodToLvUp = 0;
    private int AshToUnit = 0;
    private int AshLevel = 0;


    private int CurrentFirePercent = 1;
    private int NextFirePercent = 0;
    private int CurrentWood = 0;

    [SerializeField] float CurrentSquareRange => CurrentRange * CurrentRange;

    // 불의 범위가 변경될 때 발생하는 이벤트
    public Action OnAddWood;
    public Action OnFireRangeChanged;

    private void Start()
    {
        _fireLight = transform.GetChild(0).GetComponent<FunkyCode.Light2D>();
        Global.Instance.OnDataLoaded += _LoadFire;
    }

    public void _LoadFire()
    {
        WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID + CurrentRange - MinRange).WoodAmout;
        NextFirePercent = DataTable.Instance.GetFireLightData(StartTID + CurrentRange - MinRange).NextFirePercent;
        AshToUnit = DataTable.Instance.GetFireUnitData(StartTID + CurrentRange - MinRange).AshAmout;
        AshLevel = DataTable.Instance.GetFireUnitData(StartTID + CurrentRange - MinRange).Level;

        OnAddWood?.Invoke();
    }

    public bool CheckCanLvUP()
    {
        if(InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Wood) >= WoodToLvUp)
        {
            return true;
        }
        return false;
    }

    public bool TryToLvUP()
    {
        if (CurrentFirePercent >= NextFirePercent)
        {
            CurrentFirePercent = NextFirePercent;
            CurrentWood = 0;
            ExpandFire();
            return true;
        }
        else return false;
    }

    public bool TryAddWood()
    {
        if (InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Wood) < 1) return false;

        CurrentWood++;

        if(CurrentWood >= WoodToLvUp)
        {
            CurrentWood = 0;
            CurrentFirePercent++;
        }

        TryToLvUP();

        OnAddWood?.Invoke();
        return true;
    }

    public bool CheckCanAddUnit()
    {
        if(InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash) >= AshToUnit)
        {
            return true;
        }
        return false;
    }

    public void AshChangedAfterUnitCreate()
    {
        InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Ash, AshToUnit);
        AshToUnit = DataTable.Instance.GetFireUnitData(StartTID + AshLevel).AshAmout;
        AshLevel++;
    }

    public int GetWoodCountToLvUP()
    {
        return WoodToLvUp;
    }

    public int GetCurrentWoodCount()
    {
        return CurrentWood;
    }

    public int GetCurrentFirePercent()
    {
        return CurrentFirePercent;
    }

    public int GetAshToUnit()
    {
        return AshToUnit;
    }

    private void SetFireRange(float range)
    {
        // 범위를 설정하고 범위가 변경될 때 이벤트를 발생시킴
        // 이거 최소치보다 낮으면 죽고 높으면 성공 처리 필요
        Debug.Log("!!!!!!!!");
        CurrentRange = (int)Mathf.Clamp(range, MinRange, MaxRange);

        _fireLight.size = CurrentRange;

        WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID + CurrentRange - MinRange).WoodAmout;
        NextFirePercent = DataTable.Instance.GetFireLightData(StartTID + CurrentRange - MinRange).NextFirePercent;

        // 범위 변경 이벤트 발생
        OnFireRangeChanged?.Invoke();
    }

    public float GetRange()
    {
        return CurrentRange;
    }

    public float GetMaxRange()
    {
        return MaxRange;
    }

    public void ExpandFire()
    {
        // 범위를 증가시키고 이벤트 발생
        SetFireRange(CurrentRange + 1);
    }

    public void ReduceFire()
    {
        // 범위를 감소시키고 이벤트 발생
        SetFireRange(CurrentRange - 1);
    }

    public bool IsWithInFireRange(Vector2 position)
    {
        // 주어진 거리가 현재 범위 내에 있는지 확인
        float distance = Vector2.SqrMagnitude(position);
        return distance <= CurrentSquareRange;
    }

    public bool IsWithInFireRange(float squareDistance)
    {

        return squareDistance <= CurrentSquareRange;
    }
}
