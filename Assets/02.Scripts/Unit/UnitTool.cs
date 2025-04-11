using NavMeshPlus.Extensions;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitTool : MonoBehaviour
{
    private UnitStat _unitStat;
    private ATool _currentTool;
    public ATool CurrentTool => _currentTool;
    private ATool _prevTool;
    public ATool PrevTool => _prevTool;
    private Unit _unit;

    public bool IsInteracting;

    private Animator _animator;
    private AnimatorOverrideController _overrideController;
    private void Awake()
    {
        _unitStat = GetComponentInParent<UnitStat>();
        _unit = GetComponentInParent<Unit>();
        _animator = GetComponent<Animator>();

        _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _animator.runtimeAnimatorController = _overrideController;

        SetTool(ToolManager.Instance.GetTool(ToolType.None));
        SetTool(ToolManager.Instance.GetTool(ToolType.None));
    }

    
    public void SetTool(ATool tool)
    {
        _prevTool = _currentTool;
        _currentTool = tool;

        //_animator.SetFloat("InteractSpeed", tool.CalcAnimSpeed(_unitStat.Stat[(int)tool.StatType]);
        switch (tool.ToolType)
        {
            case ToolType.Sword:
            {
                _animator.SetFloat("InteractSpeed", _unitStat.AttackSpeed.Value / 10);
                break;
            }
            case ToolType.Axe:
            {
                _animator.SetFloat("InteractSpeed", (_unitStat.WoodSpeed.Value + _currentTool.Value)/10);
                break;
            }
            case ToolType.PickAxe:
            {
                _animator.SetFloat("InteractSpeed", (_unitStat.RockSpeed.Value + _currentTool.Value)/10);
                break;
            }
            case ToolType.Medicine:
            {
                _animator.SetFloat("interactSpeed", 1/2);
                break;
            }
        }
        _overrideController["Idle"] = ToolManager.Instance.ToolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 0];
        _overrideController["Interact"] = ToolManager.Instance.ToolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 1];

        Debug.Log($"{tool.ToolType}");
        IsInteracting = false;
        _animator.SetBool("IsInteracting", false);
    }

    public void Interact()
    {
        if(_unit.Target == null)
        {
            return;
        }
        if (!_currentTool.IsInteractable(_unit.Target.InteractType)){
            return;
        }
        _currentTool.Interact(_unit.Target, _unitStat.Damage.Value);
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
