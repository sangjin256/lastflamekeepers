public class AxeTool : ATool
{
    public AxeTool(ToolType toolType, string name, int upgradeLevel, int value) : base(toolType, name, upgradeLevel, value)
    {
    }

    public override bool IsInteractable(InteractType interactType)
    {
        if (interactType == InteractType.Tree) return true;
        // TODO: 자원 풀 확인
        return false;
    }

    public override void Interact(IInteractable interactObject, int damage)
    {
        interactObject.TakeDamage(1, false);
    }

    //public override float CalcAnimSpeed(int stat)
    //{
    //    return stat / 10;
    //}
}
