using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public GameObject CenterBaseTile;
    public List<GameObject> TilePresetList;
    public Vector2 TileSize = new Vector2(40f, 40f);

    public readonly Vector2Int[,] PositionGrid =
    {
        {new(-1,1), new(0,1), new(1,1) },
        {new(-1,0), new(0,0), new(1,0) },
        {new(-1,-1), new(0,-1), new(1,-1) },
    };

    public Transform MapRoot;
    public ResourceSpawner ResourceSpawner;

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for(int y = 0; y < 3; y++)
        {
            for(int x = 0; x < 3; x++)
            {
                Vector2Int pos = PositionGrid[y, x];
                Vector3 worldPos = new Vector3(pos.x * TileSize.x, pos.y * TileSize.y, 0f);

                GameObject selectedTile;

                if (x == 1 && y == 1) selectedTile = Instantiate(CenterBaseTile, worldPos, Quaternion.identity, MapRoot);
                else
                {
                    GameObject preset = TilePresetList[Random.Range(0, TilePresetList.Count)];
                    Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
                    selectedTile = Instantiate(preset, worldPos, rotation, MapRoot);
                }

                TileBlock block = selectedTile.GetComponent<TileBlock>();
                if(block != null)
                {
                    ResourceSpawner.SpawnResources(block);
                }
            }
        }
    }
}
