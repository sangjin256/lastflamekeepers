using UnityEngine;

public class UnitSearchTrigger : MonoBehaviour
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_unit.Target != null)
        {
            return;
        }
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        _unit.SetTarget(other.GetComponent<AInteractableEntity>());

        if (_unit.NavMeshAgent.desiredVelocity == Vector3.zero)
        {
            _unit.SetTarget(other.GetComponent<AInteractableEntity>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();

        if (interactableEntity != _unit.Target)
        {
            return;
        }

        if (!interactableEntity.CanInteract)
        {

            _unit.UnitTool.IsInteracting = false;
            _unit.ToolAnimator.SetBool("IsInteracting", false);
            _unit.NavMeshAgent.ResetPath();
            _unit.FindTarget();
            _unit.ResumeNavMeshAgent();


            if (_unit.Target == null)
            {
                _unit.NavMeshAgent.ResetPath();
            }

        }
    }

    // TODO: SearchTrigger 죽였을 때 새로 탐지, 우클릭으로 도착했을 때 새로 탐지

}
