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

    public void SpawnResources(TileBlock tile)
    {
        foreach(Transform point in tile.ResourceSpawnPoints)
        {
            float roll = Random.value;

            // 30% 확률로 바위 생성 40% 확률로 나무 생성 30% 확률로 아무것도 안함
            if(roll < 0.3f)
            {
                ClusterInstantiate(point.position, tile.gameObject, ResourceType.Rock, RockPrefabList);

                //GameObject rock = Instantiate(GetRandomPrefab(RockPrefabList), point.position, Quaternion.identity, tile.transform);

                // 리소스매니저에 넣기
            }
            else if(roll < 0.7f)
            {
                ClusterInstantiate(point.position, tile.gameObject, ResourceType.Tree, TreePrefabList);

                //GameObject wood = Instantiate(GetRandomPrefab(TreePrefabList), point.position, Quaternion.identity, tile.transform);

                // 리소스매니저에 넣기
            }
        }
    }

    private GameObject GetRandomPrefab(List<GameObject> prefabList)
    {
        return prefabList[Random.Range(0, prefabList.Count)];
    }

    private void ClusterInstantiate(Vector3 center, GameObject tile, ResourceType resourceType, List<GameObject> prefabList)
    {
        GameObject prefab = GetRandomPrefab(prefabList);

        int tries = 0;
        int placed = 0;
        int count = Random.Range(5, 24);
        float radiusX = Random.Range(1f, 6f);
        float radiusY = Random.Range(1f, 4f);

        while(placed < count && tries < count * 10)
        {
            tries++;
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0f, 1f));

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 offset = SnapToGrid(new Vector3(x, y, 0f));
            Vector3 spawnPoint = center + offset;

            bool tooClose = _placedResourcePositionList.Any(pos => Vector3.Distance(pos, spawnPoint) < _minDistance);
            if (tooClose) continue;

            _placedResourcePositionList.Add(spawnPoint);
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
