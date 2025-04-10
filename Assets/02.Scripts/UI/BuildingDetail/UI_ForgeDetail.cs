using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ForgeDetail : UI_BuildingData
{
    [Header("Tool ¿ÃπÃ¡ˆ")]
    public Image ToolImage;
    public List<Sprite> ToolSprites;
    public TextMeshProUGUI ToolCountText;

    private Forge _forge;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        _forge = (Forge)building;
        if (_forge == null)
        {
            return;
        }

        ToolImage.sprite = ToolSprites[(int)_forge.ToolType];

        RefreshUI();
    }

    protected override void RefreshUI()
    {
        base.RefreshUI();

        ToolCountText.text = $"{ToolManager.Instance.GetCurrentToolCount(_forge.ToolType)} / {ToolManager.Instance.GetMaxToolCount(_forge.ToolType)}";
    }
}
