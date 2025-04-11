using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UI_UpBar : MonoBehaviour
{
    public TextMeshProUGUI UnitCount;
    public TextMeshProUGUI SwordCount;
    public TextMeshProUGUI AxeCount;
    public TextMeshProUGUI PickAxeCount;
    public TextMeshProUGUI MedicineCount;

    public TextMeshProUGUI WoodCount;
    public TextMeshProUGUI StoneCount;
    public TextMeshProUGUI AshCount;

    public TextMeshProUGUI WaveCount;
    public Slider WaveSlider;

    public RectTransform FireImage;

    public TextMeshProUGUI FirePercent;

    private const float MaximumScreenPositionY = 37f;

    private void Start()
    {
        UnitManager.Instance.OnUnitListChanged += UnitCountRefresh;
        ToolManager.Instance.OnToolCountChanged += ToolCountRefresh;
        InventoryResourceManager.Instance.OnInvenDataChanged += InvenResourceRefresh;
        FireManager.Instance.OnFireRangeChanged += FireShadowRefresh;

        FireManager.Instance.OnPercentChanged += FirePercentRefresh;

        FireManager.Instance.OnAddWood += InvenResourceRefresh;

        WaveManager.Instance.OnEnemyDeadAction += WaveStateRefresh;
    }

    public void FirePercentRefresh()
    {
        FirePercent.text = FireManager.Instance.GetCurrentFirePercentStr();
    }

    public void FireShadowRefresh()
    {
        float nextY = FireManager.Instance.GetCurrentFirePercent() * MaximumScreenPositionY / 100;
        FireImage.DOAnchorPosY(nextY, 0.2f);
    }

    public void UnitCountRefresh(Unit unit)
    {
        UnitCount.text = $"{UnitManager.Instance.UnitList.Count.ToString()} / {UnitManager.Instance.MaxCountUnit}";
    }

    public void ToolCountRefresh()
    {
        SwordCount.text = $"{ToolManager.Instance.GetCurrentToolCount(ToolType.Sword).ToString()} / {ToolManager.Instance.GetMaxToolCount(ToolType.Sword).ToString()}";
        AxeCount.text = $"{ToolManager.Instance.GetCurrentToolCount(ToolType.Axe).ToString()} / {ToolManager.Instance.GetMaxToolCount(ToolType.Axe).ToString()}";
        PickAxeCount.text = $"{ToolManager.Instance.GetCurrentToolCount(ToolType.PickAxe).ToString()} / {ToolManager.Instance.GetMaxToolCount(ToolType.PickAxe).ToString()}";    
        MedicineCount.text = $"{ToolManager.Instance.GetCurrentToolCount(ToolType.Medicine).ToString()} / {ToolManager.Instance.GetMaxToolCount(ToolType.Medicine).ToString()}";
    }

    public void InvenResourceRefresh()
    {
        WoodCount.text = $"{InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Wood).ToString()} " +
            $"/ {InventoryResourceManager.Instance.GetMaxResourceCount(InventoryResourceType.Wood).ToString()}";
        StoneCount.text = $"{InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Stone).ToString()} " +
            $"/ {InventoryResourceManager.Instance.GetMaxResourceCount(InventoryResourceType.Stone)}";
        AshCount.text = $"{InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash).ToString()} " +
            $"/ {InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash).ToString()}";
    }

    public void WaveStateRefresh()
    {
        Debug.Log("웨이브 쿨타임을 이걸로 바꾸기");
        WaveSlider.value = WaveManager.Instance.KillCount / (float)WaveManager.Instance.WaveMaxEnemyCount;
    }
}
