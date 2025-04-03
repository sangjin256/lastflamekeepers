using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitStat : MonoBehaviour
{
    public string Name { get; private set; }
    public int MaxHealth { get; private set; }
    public int Damage { get; private set; }
    public int AttackSpeed { get; private set; }
    public int MoveSpeed { get; private set; }
    public int WoodSpeed { get; private set; }
    public int RockSpeed { get; private set; }

    private List<Feature> _goodFeatures;
    public List<Feature> GoodFeatures => _goodFeatures;
    private List<Feature> _badFeatures;
    public List<Feature> BadFeatures => _badFeatures;

    public UnitStat(int maxHealth, int attackPower, int attackSpeed, int moveSpeed, int fellingSpeed, int miningSpeed)
    {
        MaxHealth = maxHealth;
        Damage = attackPower;
        AttackSpeed = attackSpeed;
        MoveSpeed = moveSpeed;
        WoodSpeed = fellingSpeed;
        RockSpeed = miningSpeed;
    }
}
