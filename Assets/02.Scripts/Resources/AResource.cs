using UnityEngine;

public abstract class AResource : MonoBehaviour
{
    public ResourceType ResourceType;
    public string Name;
    public int Durability;
    public int AmountToHit;
    public int OutputAmountPerExtract;
    public InventoryResourceType OutputType;
    public float RespawnTime;

    public AResource(ResourceType resourceType, string name, int durability, int amountToHit,
                     int outputAmountPerExtract, InventoryResourceType outputType, float respawnTime)
    {
        ResourceType = resourceType;
        Name = name;
        Durability = durability;
        AmountToHit = amountToHit;
        OutputAmountPerExtract = outputAmountPerExtract;
        OutputType = outputType;
        RespawnTime = respawnTime;
    }
}
