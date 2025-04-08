using UnityEngine;

public class AInteractableEntity : MonoBehaviour,IInteractable
{
    protected InteractType _interactType;
    public InteractType InteractType => _interactType;


    [SerializeField]protected int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            //Action ui 갱신?
        }
    }
    //protected로
    [SerializeField]protected bool _canInteract = true;
    public bool CanInteract => _canInteract;


    public virtual void TakeDamage(int amount, bool isHeal)
    {
        Health -= (isHeal)?-amount:amount;
        if(Health <= 0)
        {
            _canInteract = false;
            //?콜라이더 disable?
        }
    }
}
