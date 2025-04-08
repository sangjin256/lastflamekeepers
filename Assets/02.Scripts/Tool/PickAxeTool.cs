using UnityEngine;

public class PickAxeTool : ATool
{
    public PickAxeTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        if (interactType == InteractType.Rock) return true;
        // TODO: 자원 풀 확인
        return false;
    }

    public override void Interact(IInteractable interactObject, int damage)
    {
        interactObject.TakeDamage(Value, false);
    }
}
