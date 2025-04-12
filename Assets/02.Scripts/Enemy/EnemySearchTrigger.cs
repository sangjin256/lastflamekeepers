using UnityEngine;
using UnityEngine.AI;

public class EnemySearchTrigger : MonoBehaviour
{
    private Enemy _enemy;
    
    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other == null)
        {
            return;
        }
        if (!other.CompareTag("Unit"))
        {
            
            return;
        }

        _enemy.FindTarget();
        //if (_enemy.NavMeshAgent.desiredVelocity == Vector3.zero)
        //{
            //_enemy.SetTarget(other.GetComponent<AInteractableEntity>());
        //}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();

        if (interactableEntity != _enemy.Target)
        {
            return;
        }
        //if (!interactableEntity.CanInteract)
        //{
            _enemy.Animator.SetBool("IsAttacking", false);
            bool flag = _enemy.NavMeshAgent.isStopped;

            _enemy.NavMeshAgent.ResetPath();
            if (flag)
            {
                _enemy.StopNavMeshAgent();
            }
            _enemy.SetTargetNull();
            _enemy.FindTarget();
        //}


    }
}