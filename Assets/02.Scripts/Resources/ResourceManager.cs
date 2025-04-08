
using System.Collections.Generic;
using UnityEngine;

// 게임 내 필드 자원 관리
// 필드 자원 : 나무(Tree), 돌(Rock)

public class ResourceManager : BehaviourSingleton<ResourceManager>
{
    private Dictionary<ResourceType, FieldResourceData> _resourceDataDict;

    public List<AResource> FieldResourceList;

    private void Awake()
    {
        Global.Instance.OnDataLoaded += LoadResourceDefinitions;
        FieldResourceList = new List<AResource>();
    }

    private void LoadResourceDefinitions()
    {
        _resourceDataDict = new Dictionary<ResourceType, FieldResourceData>();


        // 예시: DataTable에서 데이터 로드
        FieldResourceData treeData = DataTable.Instance.GetFieldResourceData(10000);
        _resourceDataDict.Add(treeData.ResourceType, treeData);

        FieldResourceData rockData = DataTable.Instance.GetFieldResourceData(10001);
        _resourceDataDict.Add(rockData.ResourceType, rockData);


        Debug.Log("Field Resource Definitions Loaded");
    }

    public bool IsResourceDataDictNotNull()
    {
        return _resourceDataDict != null;
    }

    // 필드 자원 타입 조회
    public FieldResourceData GetResource(ResourceType type)
    {
        if (_resourceDataDict.TryGetValue(type, out var resource))
            return resource;

        Debug.LogWarning($"[FieldResourceManager] 정의되지 않은 ResourceType: {type}");
        return null;
    }
}
