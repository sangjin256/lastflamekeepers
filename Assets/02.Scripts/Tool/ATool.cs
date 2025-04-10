using UnityEngine;

public abstract class ATool
{
    public ToolType ToolType;
    public string Name;
    public int UpgradeLevel;
    public int Value;

    public ATool(ToolType toolType, string name, int upgradeLevel, int value)
    {
        ToolType = toolType;
        Name = name;
        UpgradeLevel = upgradeLevel;
        Value = value;
    }

    // 도구별 기초 스탯이 시트에 없음

    public abstract bool IsInteractable(InteractType interactType);

    public abstract void Interact(IInteractable InteractableObject, int damage);
}
