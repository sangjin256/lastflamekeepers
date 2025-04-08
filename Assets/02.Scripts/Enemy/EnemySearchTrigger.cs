using UnityEngine;

public class EnemySearchTrigger : MonoBehaviour
{
    private Enemy _enemy;
    
    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //fire가 아니면
        if (_enemy.Target != null)
        {
            return;
        }
        //if (!_tool.IsInteractable(interactable.InteractType))
        //{
        //    return;
        //}

        if (!other.CompareTag("Unit"))
        {
            
            return;
        }

        //if (_enemy.NavMeshAgent.desiredVelocity == Vector3.zero)
        //{
            _enemy.SetTarget(other.GetComponent<AInteractableEntity>());
        //}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();

        if (interactableEntity != _enemy.Target)
        {
            return;
        }
        if (!interactableEntity.CanInteract)
        {
            _enemy.Animator.SetBool("IsAttacking", false);
            _enemy.NavMeshAgent.ResetPath();
            _enemy.ResumeNavMeshAgent();
            _enemy.FindTarget();
        }
    }
}