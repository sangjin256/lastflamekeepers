using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AResource : AInteractableEntity
{
    public ResourceType ResourceType;
    public string Name;
    public int Durability;
    public int AmountToHit;
    public int OutputAmountPerExtract;
    public InventoryResourceType OutputType;
    public float RespawnTime;

    public Tilemap ObstacleTilemap;

    public int HitCount = 0;
    public abstract void Initialize(ResourceType resourceType);
}
