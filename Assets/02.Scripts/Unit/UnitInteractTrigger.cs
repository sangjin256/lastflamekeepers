using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class UnitInteractTrigger : MonoBehaviour
{
    private Unit _unit;
    private UnitTool _unitTool;
    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
        _unitTool = transform.parent.GetComponentInChildren<UnitTool>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_unit.Agent.enabled)
        {
            return;
        }
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null || !interactableEntity.isActiveAndEnabled)
        {
            return;
        }
  
        if (_unit.Target !=null && interactableEntity != _unit.Target)
        {
            return;
        }
        if (!_unitTool.CurrentTool.IsInteractable(interactableEntity.InteractType))
        {
            return;
        }
        if(_unit.Target == null && interactableEntity)
        {
            if(interactableEntity.InteractType == InteractType.Enemy)
            {
                _unit.SetTarget(interactableEntity);
            }
            else
            {
                return;
            }
        }
        _unit.StopNavMeshAgent();
        _unit.ToolAnimator.SetBool("IsInteracting", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_unit.Agent.enabled)
        {
            return;
        }
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity != _unit.Target)
        {
            return;
        }

        _unitTool.IsInteracting = false;
        _unit.ToolAnimator.SetBool("IsInteracting", false);
        _unit.ResumeNavMeshAgent();
    }
}
