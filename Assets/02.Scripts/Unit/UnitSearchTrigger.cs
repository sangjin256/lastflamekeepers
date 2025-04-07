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
        //if (!_tool.IsInteractable(interactable.InteractType))
        //{
        //    return;
        //}
        if (!other.CompareTag("Enemy"))
        {
            return;
        }
        if (_unit.NavMeshAgent.desiredVelocity == Vector3.zero)
        {
            _unit.SetTarget(other.GetComponent<AInteractableEntityTest>());
        }
    }

    // TODO: SearchTrigger 죽였을 때 새로 탐지, 우클릭으로 도착했을 때 새로 탐지

}
