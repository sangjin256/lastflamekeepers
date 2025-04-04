using UnityEditorInternal;
using UnityEngine;

public class Unit : MonoBehaviour
    //:IInteractable
{
    [SerializeField] private UnitStat _unitStat;
    //private ATool _tool;

    private void Awake()
    {
        _unitStat = GetComponent<UnitStat>();
    }

    //public void Interact(IInteractable interactable)
    //{
    //    if (_tool.IsInteractable)
    //    {
    //        // TODO: NAVMESH ≈∏∞Ÿ¡ˆ¡§
    //    }
    //}

    //public override void TakeDamage(int amount, bool isHeal)
    //{

    //}
    
}
