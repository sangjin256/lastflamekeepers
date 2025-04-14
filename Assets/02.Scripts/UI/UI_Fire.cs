using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class UI_Fire : MonoBehaviour
{
    public TextMeshProUGUI FireAmount;
    public TextMeshProUGUI NeedWoodCount;

    public TextMeshProUGUI UnitCount;
    public TextMeshProUGUI AshCount;

    public GameObject UI_FireObject;
    public GameObject UI_CreateUnit;

    public Button targetButton; // 클릭 처리할 버튼
    private Coroutine clickCoroutine;
    private bool isHolding = false;

    private void Start()
    {
        UnitManager.Instance.OnUnitListChanged += UnitRefresh;
        FireManager.Instance.OnAddWood += FireRefresh;
        InventoryResourceManager.Instance.OnInvenDataChanged += FireRefresh;
    }

    public void UnitRefresh(Unit unit)
    {
        UnitCount.text = $"신도 수 {UnitManager.Instance.UnitList.Count} / {UnitManager.Instance.MaxCountUnit}";
        AshCount.text = $"필요한 잿가루 수 {InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash)} / {FireManager.Instance.GetAshToUnit()}";
    }

    public void FireRefresh()
    {
        FireAmount.text = $"밝힘 정도 {FireManager.Instance.GetCurrentFirePercentStr()} / {100}";
        NeedWoodCount.text = $"필요한 나무 수 {FireManager.Instance.GetCurrentWoodCount()} / {FireManager.Instance.GetWoodCountToLvUP()}";

        UnitCount.text = $"신도 수 {UnitManager.Instance.UnitList.Count} / {UnitManager.Instance.MaxCountUnit}";
        AshCount.text = $"필요한 잿가루 수 {InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash)} / {FireManager.Instance.GetAshToUnit()}";
    }

    public void OnClickExit()
    {
        UI_FireObject.SetActive(false);
    }

    public void OnClickAddWood()
    {
        if (FireManager.Instance.TryAddWood())
        {

        }
        else
        {
            Debug.Log("버튼 동작");
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

    public void OnPointerDown()
    {
        isHolding = true;
        clickCoroutine = StartCoroutine(HoldClickRoutine());
    }

    public void OnPointerUp()
    {
        isHolding = false;
        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
        }
    }

    private IEnumerator HoldClickRoutine()
    {
        float delay = 0.4f; // 첫 클릭까지 대기 시간
        float minDelay = 0.05f; // 최대 속도 한계
        float speedUpFactor = 0.85f; // 매번 속도가 얼마나 빨라질지

        yield return new WaitForSeconds(delay); // 첫 대기

        while (isHolding)
        {
            targetButton.onClick.Invoke(); // 버튼 클릭 호출
            delay *= speedUpFactor; // 클릭 속도 증가
            delay = Mathf.Max(delay, minDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
