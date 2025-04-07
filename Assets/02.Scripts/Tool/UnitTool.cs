using UnityEngine;
using UnityEngine.AI;

public class UnitTool : MonoBehaviour
{
    private UnitStat _unitStat;
    private ATool _tool;
    public ATool Tool => _tool;
    private Unit _unit;
    private Animator _animator;
    
    private void Awake()
    {
        _unitStat = GetComponentInParent<UnitStat>();
        _unit = GetComponentInParent<Unit>();
        _tool = ToolManager.Instance.GetTool(ToolType.Sword);
        _animator = GetComponent<Animator>();
    }

    public void SetTool(ATool tool)
    {
        _tool = tool;
    }

    public void Interact()
    {
        if(_unit.Target == null)
        {
            return;
        }
        _tool.Interact(_unit.Target, _unitStat.Damage.Value);
    }

    public void StopNavMeshAgent()
    {
        if (_unit.NavMeshAgent.enabled)
        {
            _unit.NavMeshAgent.isStopped = true;
        }
    }
    public void ResumeNavMeshAgent()
    {
        if (_unit.NavMeshAgent.enabled)
        {
            _unit.NavMeshAgent.isStopped = false;
        }
    }
}
