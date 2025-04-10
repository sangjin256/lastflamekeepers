using UnityEngine;
using TMPro;

public class UI_Fire : MonoBehaviour
{
    public TextMeshProUGUI FireAmount;
    public TextMeshProUGUI NeedWoodCount;

    public TextMeshProUGUI UnitCount;
    public TextMeshProUGUI AshCount;

    public GameObject UI_FireObject;
    public GameObject UI_CreateUnit;

    private void Start()
    {
        UnitManager.Instance.OnUnitListChanged += UnitRefresh;
        FireManager.Instance.OnFireRangeChanged += FireRefresh;
    }

    public void UnitRefresh(Unit unit)
    {
        UnitCount.text = $"신도 수 {UnitManager.Instance.UnitList.Count} / {UnitManager.Instance.MaxCountUnit}";
    }

    public void FireRefresh()
    {
        FireAmount.text = $"밝힘 정도 {FireManager.Instance.GetFirePercent()} / {100}";
        NeedWoodCount.text = $"필요한 나무 수 {FireManager.Instance.GetWoodCountToLvUP()}";
        AshCount.text = $"필요한 잿가루 수 {InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash)} / {FireManager.Instance.GetAshToUnit()}";
    }

    public void OnClickExit()
    {
        UI_FireObject.SetActive(false);
    }

    public void OnClickAddWood()
    {
        if(FireManager.Instance.CheckCanLvUP() == false)
        {
            Debug.Log("버튼 동작");
        }
        else
        {
            FireManager.Instance.TryToLvUP();
        }
    }

    public void OnClickAddAsh()
    {
        if(FireManager.Instance.CheckCanAddUnit() == false)
        {
            Debug.Log("버튼 동작");
        }
        else
        {
            UI_CreateUnit.SetActive(true);
        }
    }
}
