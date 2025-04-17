using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ResourceSpawner : MonoBehaviour
{
    public List<GameObject> TreePrefabList;
    public List<GameObject> RockPrefabList;

    public List<Vector3> _placedResourcePositionList = new List<Vector3>();

    private float _minDistance = 0.1f;
    private List<ResourceType> TypeListForInit = new List<ResourceType>();

    public void SpawnBaseResources(TileBlock tile)
    {
        bool isTreeFirst = Random.Range(0, 2) > 1f ? true : false;
        for (int i = 0; i < tile.ResourceSpawnPoints.Count; i++)
        {
            Transform point = tile.ResourceSpawnPoints[i];
            if (i == 0)
            {
                if (isTreeFirst) ClusterInstantiate(point.position, tile.gameObject, ResourceType.Tree, Random.Range(1f, 3f), Random.Range(1f, 2f), TreePrefabList);
                else ClusterInstantiate(point.position, tile.gameObject, ResourceType.Rock, Random.Range(1f, 3f), Random.Range(1f, 2f), RockPrefabList);
            }
            else if (i == 1)
            {
                if(isTreeFirst) ClusterInstantiate(point.position, tile.gameObject, ResourceType.Rock, Random.Range(1f, 3f), Random.Range(1f, 2f), RockPrefabList);
                else ClusterInstantiate(point.position, tile.gameObject, ResourceType.Tree, Random.Range(1f, 3f), Random.Range(1f, 2f), TreePrefabList);
            }
            else
            {
                float roll = Random.value;

                // 30% 확률로 바위 생성 40% 확률로 나무 생성 30% 확률로 아무것도 안함
                if (roll < 0.3f)
                {
                    ClusterInstantiate(point.position, tile.gameObject, ResourceType.Rock, Random.Range(1f, 6f), Random.Range(1f, 4f), RockPrefabList);

                    //GameObject rock = Instantiate(GetRandomPrefab(RockPrefabList), point.position, Quaternion.identity, tile.transform);

                    // 리소스매니저에 넣기
                }
                else
                {
                    ClusterInstantiate(point.position, tile.gameObject, ResourceType.Tree, Random.Range(2f, 6f), Random.Range(1f, 4f), TreePrefabList);

                    //GameObject wood = Instantiate(GetRandomPrefab(TreePrefabList), point.position, Quaternion.identity, tile.transform);

                    // 리소스매니저에 넣기
                }
            }
        }
    }

    public void SpawnResources(TileBlock tile)
    {
        foreach(Transform point in tile.ResourceSpawnPoints)
        {
            float roll = Random.value;

            // 30% 확률로 바위 생성 40% 확률로 나무 생성 30% 확률로 아무것도 안함
            if(roll < 0.3f)
            {
                ClusterInstantiate(point.position, tile.gameObject, ResourceType.Rock, Random.Range(1f, 6f), Random.Range(1f, 4f), RockPrefabList);

                //GameObject rock = Instantiate(GetRandomPrefab(RockPrefabList), point.position, Quaternion.identity, tile.transform);

                // 리소스매니저에 넣기
            }
            else if(roll < 0.75f)
            {
                ClusterInstantiate(point.position, tile.gameObject, ResourceType.Tree, Random.Range(2f, 6f), Random.Range(1f, 4f), TreePrefabList);

                //GameObject wood = Instantiate(GetRandomPrefab(TreePrefabList), point.position, Quaternion.identity, tile.transform);

                // 리소스매니저에 넣기
            }
        }
    }

    private GameObject GetRandomPrefab(List<GameObject> prefabList)
    {
        return prefabList[Random.Range(0, prefabList.Count)];
    }

    private void ClusterInstantiate(Vector3 center, GameObject tile, ResourceType resourceType, float radiusX, float radiusY, List<GameObject> prefabList)
    {
        int tries = 0;
        int placed = 0;
        int count = Random.Range(5, 24);

        while(placed < count && tries < count * 10)
        {
            tries++;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0f, 1f));

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 offset = SnapToGrid(new Vector3(x, y, 0f));
            Vector3 spawnPoint = center + offset;
            if (Global.Instance.BoundaryCheck(spawnPoint) == false) continue;
            
            bool tooClose = _placedResourcePositionList.Any(pos => Vector3.Distance(pos, spawnPoint) < _minDistance);
            if (tooClose) continue;

            _placedResourcePositionList.Add(spawnPoint);
            GameObject prefab = GetRandomPrefab(prefabList);
            AResource resource = Instantiate(prefab, spawnPoint, Quaternion.identity, tile.transform).GetComponent<AResource>();
            ResourceManager.Instance.FieldResourceList.Add(resource);
            TypeListForInit.Add(resourceType);
            placed++;
        }
    }

    public async void InitializeAllResourceAsync()
    {
        Task _init = Task.Factory.StartNew(() =>
        {
            for (; ; )
            {
                if (ResourceManager.Instance.IsResourceDataDictNotNull()) break;
            }
        });

        await _init;

        for(int i = 0; i < TypeListForInit.Count; i++)
        {
            ResourceManager.Instance.FieldResourceList[i].Initialize(TypeListForInit[i]);
        }
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
    }
}
