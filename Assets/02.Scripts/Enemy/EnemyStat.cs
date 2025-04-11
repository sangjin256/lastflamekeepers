using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private int _maxHealth;
    public int MaxHealth => _maxHealth;
    private int _damage = 20;
    public int Damage => _damage;
    private int _dropAshCount;
    public int DropAshCount => _dropAshCount;
    public void Initialize(int health, int damage, int dropAshCount)
    {
        _maxHealth = health;
        _damage = damage;
        _dropAshCount = dropAshCount;
    }
}
