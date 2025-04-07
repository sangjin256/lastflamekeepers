using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitTool : MonoBehaviour
{
    private UnitStat _unitStat;
    private ATool _tool;
    public ATool Tool => _tool;
    private Unit _unit;

    public bool IsInteracting;
    
    private void Awake()
    {
        _unitStat = GetComponentInParent<UnitStat>();
        _unit = GetComponentInParent<Unit>();
        _tool = ToolManager.Instance.GetTool(ToolType.Sword);
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
        _unit.StopNavMeshAgent();
    }
    public void ResumeNavMeshAgent()
    {
        _unit.ResumeNavMeshAgent();
    }

    public void StartInteract()
    {
        IsInteracting = true;
    }
    public void EndInteract()
    {
        IsInteracting = false;
    }
}
