using UnityEngine;

public class SwordTool : ATool
{
    public SwordTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        if (interactType == InteractType.Enemy) return true;
        return false;
    }

    public override void Interact(IInteractable interactObject, int damage)
    {
        //Value + damage
        interactObject.TakeDamage(20, false);
    }
}
