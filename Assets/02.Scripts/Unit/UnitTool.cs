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

    private AnimationClip[,,] _toolAnimationClip = new AnimationClip[3,4,2];

    public AnimationClip[] SwordAnimationClip;
    public AnimationClip[] AxeAnimationClip;
    public AnimationClip[] PickaxeAnimationClip;


    private Animator _animator;
    private AnimatorOverrideController _overrideController;
    private void Awake()
    {
        _unitStat = GetComponentInParent<UnitStat>();
        _unit = GetComponentInParent<Unit>();
        _animator = GetComponent<Animator>();

        _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _animator.runtimeAnimatorController = _overrideController;
    }

    private void Start()
    {
        InitializeAnimationClip();
    }

    public void InitializeAnimationClip()
    {

        for(int toolType = 0; toolType < _toolAnimationClip.GetLength(0); toolType++)
        {
            for (int toolLevel = 0; toolLevel < _toolAnimationClip.GetLength(1); toolLevel++)
            {
                for (int state= 0; state< _toolAnimationClip.GetLength(2); state++)
                {
                    if (toolType == 0)
                    {
                        _toolAnimationClip[toolType, toolLevel, state] = SwordAnimationClip[toolLevel * 2 + state];
                    }
                    else if (toolType == 1)
                    {
                        _toolAnimationClip[toolType, toolLevel, state] = AxeAnimationClip[toolLevel * 2 + state];
                    }
                    else if (toolType == 2)
                    {
                        _toolAnimationClip[toolType, toolLevel, state] = PickaxeAnimationClip[toolLevel * 2 + state];
                    }
                }
            }
        }

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
        _overrideController["Idle"] = _toolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 0];
        _overrideController["Interact"] = _toolAnimationClip[(int)_currentTool.ToolType, _currentTool.UpgradeLevel, 1];

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
