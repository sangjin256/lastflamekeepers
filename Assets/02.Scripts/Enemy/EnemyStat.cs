using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int Health;
    public int Damage;

    public void Initialize(int maxHealth, int damage)
    {
        Health = maxHealth;
        Damage = damage;
    }
}
