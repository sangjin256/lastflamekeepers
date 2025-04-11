using UnityEngine;

public class MedicineTool : ATool
{
    public MedicineTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        if (interactType == InteractType.Unit) return true;
        return false;
    }

    public override void Interact(IInteractable interactObject, int damage)
    {
        interactObject.TakeDamage(10, true);
    }
}
