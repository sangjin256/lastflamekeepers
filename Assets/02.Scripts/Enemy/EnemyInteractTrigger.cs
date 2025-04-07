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
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity != _enemy.Target)
        {
            return;
        }
        _enemy.StopNavMeshAgent();
        _enemy.Animator.SetBool("IsAttacking", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null)
        {
            Debug.Log("1");
            return;
        }
        if (interactableEntity != _enemy.Target)
        {
            Debug.Log("2");
            return;
        }
        Debug.Log("3");
        _enemy.Animator.SetBool("IsAttacking", false);
        if (!interactableEntity.CanInteract)
        {
            _enemy.SetTargetNull();
            _enemy.FindTarget();
        }
        _enemy.ResumeNavMeshAgent();
    }
}
