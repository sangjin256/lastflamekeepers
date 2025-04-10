using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ForgeDetail : UI_BuildingData
{
    [Header("Tool ¿ÃπÃ¡ˆ")]
    public Image ToolImage;
    public List<Sprite> ToolSprites;
    public TextMeshProUGUI ToolCountText;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        Forge forge = (Forge)building;
        if (forge == null)
        {
            return;
        }

        ToolImage.sprite = ToolSprites[(int)forge.ToolType];

        ToolCountText.text = $"{ToolManager.Instance.GetCurrentToolCount(forge.ToolType)} / {ToolManager.Instance.GetMaxToolCount(forge.ToolType)}";
    }
}
