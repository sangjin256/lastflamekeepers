using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ForgeDetail : UI_BuildingData
{
    [Header("Tool ¿ÃπÃ¡ˆ")]
    public Image ToolImage;
    public List<Sprite> ToolSprites;

    public override void Initialize(ABaseBuilding building)
    {
        base.Initialize(building);

        Forge forge = (Forge)building;
        if (forge == null)
        {
            return;
        }

        ToolImage.sprite = ToolSprites[(int)forge.ToolType];
    }
}
