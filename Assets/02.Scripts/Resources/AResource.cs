using Unity.VisualScripting.InputSystem;
using UnityEngine;

public abstract class AResource : MonoBehaviour, IInteractable
{
    public ResourceType ResourceType;
    public string Name;
    public int Durability;
    public int AmountToHit;
    public int OutputAmountPerExtract;
    public InventoryResourceType OutputType;
    public float RespawnTime;

    public void Initialize(ResourceType resourceType)
    {
        AResource resource = ResourceManager.Instance.GetResource(resourceType);
        ResourceType = resource.ResourceType;
        Name = resource.Name;
        Durability = resource.Durability;
        AmountToHit = resource.AmountToHit;
        OutputAmountPerExtract = resource.OutputAmountPerExtract;
        OutputType = resource.OutputType;
        RespawnTime = resource.RespawnTime;
    }

    public void TakeDamage(int amount, bool isHeal)
    {

    }
}
