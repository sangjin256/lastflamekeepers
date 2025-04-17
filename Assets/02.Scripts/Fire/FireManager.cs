using FunkyCode;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

//  불의 범위를 관리하고 범위가 변경될 때 이벤트를 발생시키는 클래스


public class FireManager : BehaviourSingleton<FireManager>
{
    private Fire _fire;

    [Header("불 범위 설정")]
    [SerializeField] int MaxRange = 43;
    [SerializeField] const int MinRangeDeath = 3;
    [SerializeField] const int MinRangeData = 8;
    [SerializeField] float CurrentRange = 8f;
    [SerializeField] int MinFirePercentData = 1;
    [SerializeField] int MaxFirePercentData = 100;

    private FunkyCode.Light2D _fireLight;

    private const int StartTID = 10000;
    private int WoodToLvUp = 0;
    private int AshToUnit = 0;
    private int AshLevel = 0;


    private float CurrentFirePercent = 1f;
    private float NextFirePercent = 0;
    private float PrevFirePercent = 0;
    private int CurrentWood = 0;

    private int CurrentTID;

    [SerializeField] float CurrentSquareRange => CurrentRange * CurrentRange;

    // 불의 범위가 변경될 때 발생하는 이벤트
    public Action OnAddWood;
    public Action OnFireRangeChanged;
    public Action OnPercentChanged;

    private void Start()
    {
        _fire = FindAnyObjectByType<Fire>();
        _fireLight = transform.GetChild(0).GetComponent<FunkyCode.Light2D>();
        Global.Instance.OnDataLoaded += _LoadFire;
    }

    public AInteractableEntity GetFire()
    {
        return _fire;
    }
    public void _LoadFire()
    {
        CurrentTID = StartTID;
        WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID).WoodAmout;
        NextFirePercent = DataTable.Instance.GetFireLightData(StartTID).NextFirePercent;
        AshToUnit = DataTable.Instance.GetFireUnitData(StartTID).AshAmout;
        AshLevel = DataTable.Instance.GetFireUnitData(StartTID).Level;
        PrevFirePercent = NextFirePercent;
        OnAddWood?.Invoke();
        OnPercentChanged?.Invoke();
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
        InventoryResourceManager.Instance.TryRemoveCurrentResourceCount(InventoryResourceType.Wood, 1);
        if(CurrentWood >= WoodToLvUp)
        {
            CurrentWood = 0;
            CurrentFirePercent++;
            OnPercentChanged?.Invoke();

            ExpandFire();
            if (CurrentFirePercent >= NextFirePercent)
            {
            }
        }

        OnAddWood?.Invoke();
        return true;
    }

    public bool CheckCanAddUnit()
    {
        if(InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash) >= AshToUnit && UnitManager.Instance.UnitList.Count < UnitManager.Instance.MaxCountUnit)
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

    public float GetCurrentFirePercent()
    {
        return CurrentFirePercent;
    }

    public string GetCurrentFirePercentStr()
    {
        string result;

        if (CurrentFirePercent < 1) result = CurrentFirePercent.ToString("0.0");
        else result = CurrentFirePercent.ToString("0");

        return result;
    }

    public int GetAshToUnit()
    {
        return AshToUnit;
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
        if (CurrentFirePercent >= MaxFirePercentData)
        {
            GameManager.Instance.Success();
            return;
        }

        if (CurrentFirePercent <= MinFirePercentData)
        {
            CurrentFirePercent = 1;
            CurrentRange = MinRangeData;
            DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);
            WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID).WoodAmout;
            NextFirePercent = DataTable.Instance.GetFireLightData(StartTID).NextFirePercent;

            return;
        }

        CurrentRange += 0.3f;
        DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);

        if (CurrentFirePercent >= NextFirePercent)
        {
            PrevFirePercent = NextFirePercent;
            CurrentTID++;

            WoodToLvUp = DataTable.Instance.GetFireLightData(CurrentTID).WoodAmout;
            NextFirePercent = DataTable.Instance.GetFireLightData(CurrentTID).NextFirePercent;
        }

        //if (CurrentFirePercent >= MaxFirePercentData)
        //{
        //    GameManager.Instance.Success();
        //    return;
        //}

        //if (CurrentFirePercent <= MinFirePercentData)
        //{

        //    CurrentFirePercent = 1;
        //    CurrentRange = MinRangeData;
        //    DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);
        //    WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID).WoodAmout;
        //    NextFirePercent = DataTable.Instance.GetFireLightData(StartTID).NextFirePercent;

        //    return;
        //}

        //if (CurrentFirePercent >= NextFirePercent)
        //{
        //    PrevFirePercent = NextFirePercent;
        //    CurrentRange += 2;
        //    CurrentTID++;

        //    DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);
        //    WoodToLvUp = DataTable.Instance.GetFireLightData(CurrentTID).WoodAmout;
        //    NextFirePercent = DataTable.Instance.GetFireLightData(CurrentTID).NextFirePercent;
        //}

        OnFireRangeChanged?.Invoke();
    }

    public void ReduceFire()
    {
        if (CurrentFirePercent <= MinFirePercentData)
        {
            if (CurrentFirePercent - 0.2f <= 0.1f)
            {
                GameManager.Instance.Defeat();
                return;
            }

            CurrentFirePercent -= 0.2f;
            CurrentRange -= 0.3f;
            CurrentWood = 0;
            DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);
            WoodToLvUp = DataTable.Instance.GetFireLightData(StartTID).WoodAmout;
            NextFirePercent = DataTable.Instance.GetFireLightData(StartTID).NextFirePercent;

            OnAddWood?.Invoke();
            OnFireRangeChanged?.Invoke();
            OnPercentChanged?.Invoke();

            return;
        }

        CurrentFirePercent--;
        CurrentRange -= 0.3f;

        DOTween.To(() => _fireLight.size, x => _fireLight.size = x, CurrentRange, 0.5f).SetEase(Ease.OutCubic);

        if (CurrentFirePercent <= PrevFirePercent)
        {
            CurrentWood = 0;
            CurrentTID--;
            if (CurrentTID <= StartTID) CurrentTID = StartTID;
            NextFirePercent = PrevFirePercent;
            WoodToLvUp = DataTable.Instance.GetFireLightData(CurrentTID).WoodAmout;
            PrevFirePercent = DataTable.Instance.GetFireLightData(CurrentTID).NextFirePercent;
        }

        OnAddWood?.Invoke();
        OnFireRangeChanged?.Invoke();
        OnPercentChanged?.Invoke();
    }

    public bool IsWithInFireRange(Vector2 position)
    {
        // 주어진 거리가 현재 범위 내에 있는지 확인
        float distance = Vector2.SqrMagnitude(position);
        return distance <= CurrentSquareRange;
    }

    public bool IsUnitWithInFireRange(Vector2 position)
    {
        float distance = Vector2.SqrMagnitude(position);
        return distance <= CurrentSquareRange - 10;
    }

    public bool IsWithInFireRange(float squareDistance)
    {

        return squareDistance <= CurrentSquareRange;
    }
}
