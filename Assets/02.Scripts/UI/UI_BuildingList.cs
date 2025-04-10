using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UI_BuildingList : MonoBehaviour
{
    [Header("프리팹들의 부모 오브젝트")]
    public GameObject BuildPrefab_Layout;

    [Header("빌딩 리스트 엘리먼트 프리팹")]
    public GameObject BuildList_BuildPrefab;
    private Dictionary<BuildingType, List<UI_BuildingListElement>> _buildingElements;

    private void Start()
    {
        BuildManager.Instance.OnChangeBuildingList += RefreshBuildingList;

        _buildingElements = new Dictionary<BuildingType, List<UI_BuildingListElement>>();
        for (int i = 0; i <= (int)BuildingType.Forge; i++)
        {
            _buildingElements[(BuildingType)i] = new List<UI_BuildingListElement>();
        }
    }


    private void RefreshBuildingList(ABaseBuilding building)
    {
        List<UI_BuildingListElement> buildingElementList = _buildingElements[building.BuildingType];
        for (int i = 0; i < buildingElementList.Count; i++)
        {
            if (buildingElementList[i].Building == building)
            {
                buildingElementList[i].Refresh();
                buildingElementList.Sort(CompareBuilding);
                SortNode(buildingElementList, building.BuildingType);
                return;
            }
        }

        GameObject buildingElementObject = Instantiate(BuildList_BuildPrefab);
        UI_BuildingListElement buildingElement = buildingElementObject.GetComponent<UI_BuildingListElement>();

        if (buildingElement == null)
        {
            return;
        }

        buildingElement.transform.SetParent(BuildPrefab_Layout.transform);
        buildingElement.transform.localScale = Vector3.one;
        buildingElement.transform.position = Vector3.zero;

        buildingElement.SetBuilding(building);
        buildingElementList.Add(buildingElement);
        SortNode(buildingElementList, building.BuildingType);
    }

    private int CompareBuilding(UI_BuildingListElement a, UI_BuildingListElement b)
    {
        return a.Building.Level.CompareTo(b.Building.Level);
    }

    private void SortNode(List<UI_BuildingListElement> buildingElementList, BuildingType buildingType)
    {
        int sum = 0;
        for (int i = 0; i < (int)buildingType; i++)
        {
            sum += _buildingElements[(BuildingType)i].Count;
        }

        for (int i = 0; i < buildingElementList.Count; i++)
        {
            buildingElementList[i].transform.SetSiblingIndex(sum + i);
        }
    }
}
