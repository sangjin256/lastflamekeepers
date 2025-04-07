using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private int _maxHealth;
    public int MaxHealth => _maxHealth;
    private int _damage;
    public int Damage => _damage;
    public void Initialize(int health, int damage)
    {
        _maxHealth = health;
        _damage = damage;
    }
}
