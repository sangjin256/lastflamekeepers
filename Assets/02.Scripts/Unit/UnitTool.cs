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
    private Unit _unit;

    public bool IsInteracting;


    public List<AnimationClip> SwordAnimationClip;
    public List<AnimationClip> AxeAnimationClip;
    public List<AnimationClip> PickaxeAnimationClip;

    public AnimationClip[,,] ToolAnimationClip;

    private Animator _animator;
    public AnimatorOverrideController overrideController;
    private void Awake()
    {
        _unitStat = GetComponentInParent<UnitStat>();
        _unit = GetComponentInParent<Unit>();
        _animator = GetComponent<Animator>();

        overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _animator.runtimeAnimatorController = overrideController;
    }

    private void Start()
    {
        SetTool(ToolManager.Instance.GetTool(ToolType.Sword));
    }

    public void SetTool(ATool tool)
    {
        _currentTool = tool;

        switch (tool.ToolType)
        {
            case ToolType.Sword:
            {
                _animator.SetFloat("InteractSpeed", _unitStat.AttackSpeed.Value / 10);
                break;
            }
            case ToolType.Axe:
            {
                _animator.SetFloat("InteractSpeed", _unitStat.WoodSpeed.Value / 10 + _currentTool.Value);
                break;
            }
            case ToolType.PickAxe:
            {
                _animator.SetFloat("InteractSpeed", _unitStat.RockSpeed.Value / 10 + _currentTool.Value);
                break;
            }
        }
        overrideController["Idle"] = ToolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 0];
        overrideController["Interact"] = ToolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 1];

        IsInteracting = false;
        _animator.SetBool("IsInteracting", false);
    }

    public void Interact()
    {
        if(_unit.Target == null)
        {
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
