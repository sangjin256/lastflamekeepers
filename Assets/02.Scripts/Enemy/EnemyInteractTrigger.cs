using UnityEngine;
using UnityEngine.AI;
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
        if (!interactableEntity.CompareTag("Unit") && !interactableEntity.CompareTag("Fire"))
        {
            return;
        }
        if (interactableEntity != _enemy.Target)
        {
            //if (_enemy.Target == FireManager.Instance.GetFire())
            //{
            //    _enemy.SetTarget(interactableEntity);
            //}
            _enemy.SetTarget(interactableEntity);
            return;
        }
        _enemy.StopNavMeshAgent();
        _enemy.Animator.SetBool("IsAttacking", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        //이럴상황이?
        if (interactableEntity == null)
        {
            return;
        }
        if (!interactableEntity.CompareTag("Unit"))
        {
            return;
        }
        if (interactableEntity != _enemy.Target)
        {
            return;
        }

        _enemy.Animator.SetBool("IsAttacking", false);


        //도망가거나 죽은 경우 새로운 대상 탐색
        //if (!interactableEntity.CanInteract)




        //_enemy.ResumeNavMeshAgent();
    }
}
