using Unity.VisualScripting;
using UnityEngine;

public class UnitInteractTrigger : MonoBehaviour
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity != _unit.Target)
        {
            return;
        }
        _unit.StopNavMeshAgent();
        _unit.ToolAnimator.SetBool("IsInteracting", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        AInteractableEntity interactableEntity = other.GetComponent<AInteractableEntity>();
        if (interactableEntity == null)
        {
            return;
        }
        if (interactableEntity != _unit.Target)
        {
            return;
        }

        Debug.Log($"{transform.parent.gameObject.name}: SSSS");
        _unit.UnitTool.IsInteracting = false;
        _unit.ToolAnimator.SetBool("IsInteracting", false);
        _unit.ResumeNavMeshAgent();
    }
}
