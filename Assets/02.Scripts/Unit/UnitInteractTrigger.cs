using Unity.VisualScripting;
using UnityEngine;

public class UnitInteractTrigger : MonoBehaviour
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    AInteractableEntityTest interactableEntity = other.GetComponent<AInteractableEntityTest>();
    //    if (interactableEntity == null)
    //    {
    //        return;
    //    }
    //    if (interactableEntity != _unit.Target)
    //    {
    //        return;
    //    }
    //    //Interact(_target) -> _tool.Interact(interactableEntity, _tool.Value);
    //    _unit.StopNavMeshAgent();

    //    _unit.Interact(_unit.Target);
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    AInteractableEntityTest interactableEntity = other.GetComponent<AInteractableEntityTest>();
    //    if (interactableEntity != _unit.Target)
    //    {
    //        return;
    //    }

    //    _unit.ToolAnimator.SetBool("IsInteracting", false);
    //    _unit.ResumeNavMeshAgent();
    //}
}
