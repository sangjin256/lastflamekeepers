using Unity.VisualScripting;
using UnityEngine;

public class AxeTool : ATool
{
    public AxeTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        if (interactType == InteractType.Wood) return true;
        return false;
    }

    public override void Interact(IInteractable interactObject, int damage)
    {
        interactObject.TakeDamage(Value + damage, false);
    }
}
