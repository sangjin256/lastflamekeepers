using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    private void Start()
    {
        UnitManager.Instance.OnUnitCountChanged += UnitCountRefresh;
        ToolManager.Instance.OnToolDataChanged += ToolCountRefresh;
        InventoryResourceManager.Instance.OnInvenDataChanged += InvenResourceRefresh;

        WaveManager.Instance.OnEnemyDeadAction += WaveStateRefresh;
    }

    public void UnitCountRefresh()
    {
        UnitCount.text = UnitManager.Instance.UnitList.Count.ToString();
    }

    public void ToolCountRefresh()
    {
        SwordCount.text = ToolManager.Instance.GetCurrentToolCount(ToolType.Sword).ToString();
        AxeCount.text = ToolManager.Instance.GetCurrentToolCount(ToolType.Axe).ToString();
        PickAxeCount.text = ToolManager.Instance.GetCurrentToolCount(ToolType.PickAxe).ToString();
        MedicineCount.text = ToolManager.Instance.GetCurrentToolCount(ToolType.Medicine).ToString();
    }

    public void InvenResourceRefresh()
    {
        WoodCount.text = InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Wood).ToString();
        StoneCount.text = InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Stone).ToString();
        AshCount.text = InventoryResourceManager.Instance.GetCurrentResourceCount(InventoryResourceType.Ash).ToString();
    }

    public void WaveStateRefresh()
    {
        WaveSlider.value = WaveManager.Instance.KillCount / (float)WaveManager.Instance.WaveMaxEnemyCount;
    }
}
