using Unity.VisualScripting.InputSystem;
using UnityEngine;

public abstract class AResource : AInteractableEntity
{
    public ResourceType ResourceType;
    public string Name;
    public int Durability;
    public int AmountToHit;
    public int OutputAmountPerExtract;
    public InventoryResourceType OutputType;
    public float RespawnTime;

    public int HitCount = 0;
    public const int HitMaxCount = 3;

    public void Initialize(ResourceType resourceType)
    {
        FieldResourceData resource = ResourceManager.Instance.GetResource(resourceType);
        ResourceType = resource.ResourceType;
        Name = resource.Name;
        Durability = resource.Durability;
        AmountToHit = resource.AmountToHit;
        OutputAmountPerExtract = resource.OutputAmountPerExtract;
        OutputType = resource.OutputType;
        RespawnTime = resource.RespawnTime;
    }
}
