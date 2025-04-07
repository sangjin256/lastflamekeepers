
using System.Collections.Generic;
using UnityEngine;

// 게임 내 필드 자원 관리
// 필드 자원 : 나무(Tree), 돌(Rock)

public class ResourceManager : BehaviourSingleton<ResourceManager>
{
    private Dictionary<ResourceType, AResource> _resourceDataDict;

    private List<AResource> _FieldResourceList;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += LoadResourceDefinitions;
    }

    private void LoadResourceDefinitions()
    {
        _resourceDataDict = new Dictionary<ResourceType, AResource>();


        // 예시: DataTable에서 데이터 로드
        FieldResourceData treeData = DataTable.Instance.GetFieldResourceData(10000);
        _resourceDataDict.Add(treeData.ResourceType,
            new Tree(treeData.ResourceType, treeData.Name, treeData.Durability,
                             treeData.AmountToHit, treeData.OutputAmountPerExtract,
                             treeData.OutputType, treeData.RespawnTime));

        FieldResourceData rockData = DataTable.Instance.GetFieldResourceData(10001);
        _resourceDataDict.Add(rockData.ResourceType,
            new Rock(rockData.ResourceType, rockData.Name, rockData.Durability,
                             rockData.AmountToHit, rockData.OutputAmountPerExtract,
                             rockData.OutputType, rockData.RespawnTime));


        Debug.Log("Field Resource Definitions Loaded");
    }

    // 필드 자원 타입 조회
    public AResource GetResource(ResourceType type)
    {
        if (_resourceDataDict.TryGetValue(type, out var resource))
            return resource;

        Debug.LogWarning($"[FieldResourceManager] 정의되지 않은 ResourceType: {type}");
        return null;
    }

    public List<AResource> GetFieldResourceList()
    {
        return _FieldResourceList;
    }

    public void SetFieldResourceList(List<AResource> list)
    {
        _FieldResourceList = list;
    }
}
