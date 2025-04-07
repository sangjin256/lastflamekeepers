using UnityEngine;

public class AInteractableEntityTest : MonoBehaviour,IInteractable
{
    private InteractType _interactType;
    public InteractType InteractType => _interactType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(int amount, bool isHeal)
    {
    }
}
