using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using UnityEngine;

public class UnitStat : MonoBehaviour
{
    public string Name;
    public Stat MaxHealth { get; private set; }
    public Stat Damage { get; private set; }
    public Stat AttackSpeed { get; private set; }
    public Stat MoveSpeed { get; private set; }
    public Stat WoodSpeed { get; private set; }
    public Stat RockSpeed { get; private set; }

    private List<Feature> _positiveFeatureList;
    public List<Feature> PositiveFeatureList => _positiveFeatureList;
    private List<Feature> _negativeFeatureList;
    public List<Feature> NegativeFeatureList => _negativeFeatureList;

    public void AllocateFeatureList()
    {
        _positiveFeatureList = new List<Feature>();
        _negativeFeatureList = new List<Feature>();
    }
    public void AddPositiveFeature(Feature feature)
    {
        _positiveFeatureList.Add(feature);
        MaxHealth = new Stat(MaxHealth.Value, MaxHealth.AddedValue + feature.MaxHealthValue);
        Damage = new Stat(Damage.Value, Damage.AddedValue + feature.DamageValue);
        AttackSpeed = new Stat(AttackSpeed.Value, AttackSpeed.AddedValue + feature.AttackSpeedValue);
        MoveSpeed = new Stat(MoveSpeed.Value, MoveSpeed.AddedValue + feature.MoveSpeedValue);
        WoodSpeed = new Stat(WoodSpeed.Value, WoodSpeed.AddedValue + feature.WoodSpeedValue);
        RockSpeed = new Stat(RockSpeed.Value, RockSpeed.AddedValue + feature.RockSpeedValue);

    }
    public void AddNegativeFeature(Feature feature)
    {
        _negativeFeatureList.Add(feature);
        MaxHealth = new Stat(MaxHealth.Value, MaxHealth.AddedValue + feature.MaxHealthValue);
        Damage = new Stat(Damage.Value, Damage.AddedValue + feature.DamageValue);
        AttackSpeed = new Stat(AttackSpeed.Value, AttackSpeed.AddedValue + feature.AttackSpeedValue);
        MoveSpeed = new Stat(MoveSpeed.Value, MoveSpeed.AddedValue + feature.MoveSpeedValue);
        WoodSpeed = new Stat(WoodSpeed.Value, WoodSpeed.AddedValue + feature.WoodSpeedValue);
        RockSpeed = new Stat(RockSpeed.Value, RockSpeed.AddedValue + feature.RockSpeedValue);
    }
    public void DebugStat()
    {
        Debug.Log($"MaxHealth: {MaxHealth.Value}({MaxHealth.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"Damage: {Damage.Value}({Damage.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"AttakSpeed: {AttackSpeed.Value}({AttackSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"MoveSpeed: {MoveSpeed.Value}({MoveSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"WoodSpeed: {WoodSpeed.Value}({WoodSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"RockSpeed: {RockSpeed.Value}({RockSpeed.AddedValue.ToString("+#;-#;0")})");

        foreach (Feature feature in PositiveFeatureList)
        {
            Debug.Log($"编沥利 漂己: {feature.FeatureName}");
        }
        foreach (Feature feature in NegativeFeatureList)
        {
            Debug.Log($"何沥利 漂己: {feature.FeatureName}");
        }
    }
    public void UnitStatInitialize(int maxHealth, int damage, int attackSpeed, int moveSpeed, int woodSpeed, int rockSpeed)
    {
        MaxHealth = new Stat(maxHealth);
        Damage = new Stat(damage);
        AttackSpeed = new Stat(attackSpeed);
        MoveSpeed = new Stat(moveSpeed);
        WoodSpeed = new Stat(woodSpeed);
        RockSpeed = new Stat(rockSpeed);
    }

}
