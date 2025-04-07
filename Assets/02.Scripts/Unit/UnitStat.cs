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

    private List<Feature> _positiveFeatureList = new List<Feature>();
    public List<Feature> PositiveFeatureList => _positiveFeatureList;
    private List<Feature> _negativeFeatureList = new List<Feature>();
    public List<Feature> NegativeFeatureList => _negativeFeatureList;


    public void AddPositiveFeature(Feature feature)
    {
        _positiveFeatureList.Add(feature);
        MaxHealth.AddedValue += feature.MaxHealthValue;
        Damage.AddedValue += feature.DamageValue;
        AttackSpeed.AddedValue += feature.AttackSpeedValue;
        MoveSpeed.AddedValue += feature.MoveSpeedValue;
        WoodSpeed.AddedValue += feature.WoodSpeedValue;
        RockSpeed.AddedValue += feature.RockSpeedValue;

    }
    public void AddNegativeFeature(Feature feature)
    {
        _negativeFeatureList.Add(feature);
        MaxHealth.AddedValue += feature.MaxHealthValue;
        Damage.AddedValue += feature.DamageValue;
        AttackSpeed.AddedValue += feature.AttackSpeedValue;
        MoveSpeed.AddedValue += feature.MoveSpeedValue;
        WoodSpeed.AddedValue += feature.WoodSpeedValue;
        RockSpeed.AddedValue += feature.RockSpeedValue;
    }
    public void DebugStat()
    {
        Debug.Log($"MaxHealth: {MaxHealth.BaisicValue}({MaxHealth.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"Damage: {Damage.BaisicValue}({Damage.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"AttakSpeed: {AttackSpeed.BaisicValue}({AttackSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"MoveSpeed: {MoveSpeed.BaisicValue}({MoveSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"WoodSpeed: {WoodSpeed.BaisicValue}({WoodSpeed.AddedValue.ToString("+#;-#;0")})");
        Debug.Log($"RockSpeed: {RockSpeed.BaisicValue}({RockSpeed.AddedValue.ToString("+#;-#;0")})");

        foreach (Feature feature in PositiveFeatureList)
        {
            Debug.Log($"编沥利 漂己: {feature.FeatureName}");
        }
        foreach (Feature feature in NegativeFeatureList)
        {
            Debug.Log($"何沥利 漂己: {feature.FeatureName}");
        }
    }
    public void Initialize(int maxHealth, int damage, int attackSpeed, int moveSpeed, int woodSpeed, int rockSpeed)
    {
        MaxHealth = new Stat(maxHealth);
        Damage = new Stat(damage);
        AttackSpeed = new Stat(attackSpeed);
        MoveSpeed = new Stat(moveSpeed);
        WoodSpeed = new Stat(woodSpeed);
        RockSpeed = new Stat(rockSpeed);
    }

}
