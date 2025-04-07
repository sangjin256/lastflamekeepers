using System;

public class Feature
{
    public FeatureType FeatureType { get; private set; }
    public FeatureType ConflicitngFeatureType { get; private set; }

    public string FeatureName { get; private set; }
    public string Description { get; private set; }
    public int MaxHealthValue { get; private set; }
    public int DamageValue { get; private set; }
    public int AttackSpeedValue { get; private set; }
    public int MoveSpeedValue { get; private set; }
    public int WoodSpeedValue { get; private set; }
    public int RockSpeedValue { get; private set; }
    public Feature(FeatureData data)
    {
        FeatureType = data.FeatureType;
        FeatureName = data.FeatureName;
        Description = data.Description;
        MaxHealthValue = data.MaxHealthValue;
        DamageValue = data.DamageValue;
        AttackSpeedValue = data.AttackSpeedValue;
        MoveSpeedValue = data.MoveSpeedValue;
        WoodSpeedValue = data.WoodSpeedValue;
        RockSpeedValue = data.RockSpeedValue;
    }
}
