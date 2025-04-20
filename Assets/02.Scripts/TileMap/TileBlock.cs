using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TileBlock : MonoBehaviour
{
    public List<Transform> ResourceSpawnPoints;

    public Transform ResourcePointsRoot;

    public Tilemap ObstacleTilemap;

    private void Awake()
    {
        for(int i = 0; i < ResourcePointsRoot.childCount; i++)
        {
            ResourceSpawnPoints.Add(ResourcePointsRoot.GetChild(i).transform);
        }
    }
}
