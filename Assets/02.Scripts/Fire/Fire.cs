using UnityEngine;

public class Fire : AInteractableEntity
{
    [ContextMenu("TAKEDAMAGE")]
    public void DMG()
    {
        TakeDamage(1, false);
    }

    public override void TakeDamage(int amount, bool isHeal)
    {
        FireManager.Instance.ReduceFire();
    }
}
