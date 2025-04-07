using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyInteractTrigger : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AInteractableEntityTest interactableEntity = other.GetComponent<AInteractableEntityTest>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity.transform != _enemy.Target)
        {
            return;
        }
        _enemy.IsAttacking = true;
        _enemy.Animator.SetBool("IsAttacking", _enemy.IsAttacking);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntityTest interactableEntity = other.GetComponent<AInteractableEntityTest>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity.transform != _enemy.Target)
        {
            return;
        }

        _enemy.IsAttacking = false;
        _enemy.Animator.SetBool("IsAttacking", _enemy.IsAttacking);
    }
}
