using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HospitalDetail : UI_BuildingData
{
    [Header("Tool ¿ÃπÃ¡ˆ")]
    public Image MedicineImage;
    public Sprite MedicineSprite;
    public TextMeshProUGUI MedicineCountText;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        MedicineImage.sprite = MedicineSprite;

        MedicineCountText.text = 
            $"{ToolManager.Instance.GetCurrentToolCount(ToolType.Medicine)} / {ToolManager.Instance.GetMaxToolCount(ToolType.Medicine)}";
    }

    protected override void RefreshUI()
    {
        base.RefreshUI();

        MedicineCountText.text =
            $"{ToolManager.Instance.GetCurrentToolCount(ToolType.Medicine)} / {ToolManager.Instance.GetMaxToolCount(ToolType.Medicine)}";
    }
}
