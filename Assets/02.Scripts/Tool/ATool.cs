using UnityEngine;

public abstract class ATool
{
    public ToolType ToolType;
    public string Name;
    public int UpgradeLevel;
    public int Value;

    // 도구별 기초 스탯이 시트에 없음

    public abstract void Interact();
}
