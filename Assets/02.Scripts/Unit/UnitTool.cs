using System.Collections.Generic;
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


    public List<AnimationClip> SwordAnimationClip;
    public List<AnimationClip> AxeAnimationClip;
    public List<AnimationClip> PickaxeAnimationClip;

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
        SetTool(ToolManager.Instance.GetTool(ToolType.Axe));
    }

    public void SetTool(ATool tool)
    {
        _tool = tool;

        switch (tool.ToolType)
        {
            case ToolType.Sword:
            {
                overrideController["Idle"] = SwordAnimationClip[0];
                overrideController["Interact"] = SwordAnimationClip[1];

                _animator.SetFloat("InteractSpeed", _unitStat.AttackSpeed.Value / 10);
                break;
            }
            case ToolType.Axe:
            {
                overrideController["Idle"] = AxeAnimationClip[0];
                overrideController["Interact"] = AxeAnimationClip[1];
                _animator.SetFloat("InteractSpeed", _unitStat.WoodSpeed.Value / 10 + _tool.Value);
                break;
            }
            case ToolType.PickAxe:
            {
                overrideController["Idle"] = PickaxeAnimationClip[0];
                overrideController["Interact"] = PickaxeAnimationClip[1];
                _animator.SetFloat("InteractSpeed", _unitStat.RockSpeed.Value / 10 + _tool.Value);

                break;
            }
        }

        IsInteracting = false;
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
