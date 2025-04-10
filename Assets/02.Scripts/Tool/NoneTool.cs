using UnityEngine;

public class NoneTool : ATool
{
    public NoneTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        return false;
    }

    public override void Interact(IInteractable InteractableObject, int damage)
    {
        return;
    }


}
