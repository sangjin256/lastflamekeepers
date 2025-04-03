using System.Collections.Generic;
using UnityEngine;

public class BuildManager : BehaviourSingleton<BuildManager>
{
    [Header("빌딩 프리팹")]
    public List<GameObject> BuildingPrefabs;        // 배치할 건물 프립팹
    private Building _previewBuilding = null;     // 프리뷰용 건물

    [Header("프리뷰 건물 색")]
    public Color CanBuildColor = Color.white;
    public Color CannotBuildColor = Color.red;


    // 소환된 빌딩들
    private List<Building> _buildings;
    // 테스트용 거리 제한
    public float MaxBuildDistance = 5f;

    private void Update()
    {
        // 현재 건물 짓는 모드인지 확인
        if (_previewBuilding == null)
        {
            return;
        }
        
        // 마우스에 따라서 프리뷰 움직이기
        MovePreviewBuildingToMouse();

        // 건물 지을 수 있는 지 확인
        if (CheckBuildCondition())
        {
            
            _previewBuilding.ChangeColor(CanBuildColor);
            if (Input.GetMouseButtonDown(0))
            {
                TryBuild();
            }
            
        }
        else
        {
            _previewBuilding.ChangeColor(CannotBuildColor);
        }
    }

    private void MovePreviewBuildingToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _previewBuilding.transform.position = mousePosition;
    }

    private bool CheckBuildCondition()
    {
        // 1. 다른 건물과 충돌 여부 확인
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_previewBuilding.transform.position, _previewBuilding.GetComponent<BoxCollider2D>().size, 0);
        foreach (Collider2D collider in colliders)
        {
            Building building = collider.GetComponent<Building>();
            if (building && building != _previewBuilding)
            {
                Debug.Log(1);
                return false;
            }
        }

        // 불에서부터의 거리 확인
        float distanceFromFire = Vector2.Distance(Vector2.zero, _previewBuilding.transform.position);
        if (distanceFromFire > MaxBuildDistance)
        {
            Debug.Log(2);
            return false;
        }

        Debug.Log(3);
        return true;
    }

    private void TryBuild()
    {
        // 새로운 건물 소환
        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)_previewBuilding.BuildingType]);
        newBuilding.transform.position = _previewBuilding.transform.position;

        // 건물 관리용 리스트에 추가
        Building building = newBuilding.GetComponent<Building>();
        if (building == null)
        {
            return;
        }

        _buildings.Add(building);

        // 기존 프리뷰 건물 제거
        Destroy(_previewBuilding.gameObject);
        _previewBuilding = null;
    }

    public void StartBuildingMode(BuildingType buildingType)
    {
        if (_previewBuilding != null)
        {
            return;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)buildingType]);
        _previewBuilding = newBuilding.GetComponent<Building>();
    }

    public void StartBuildingHouse()
    {
        if (_previewBuilding != null)
        {
            return;
        }

        GameObject newBuilding = Instantiate(BuildingPrefabs[(int)BuildingType.House]);
        _previewBuilding = newBuilding.GetComponent<Building>();
    }
}
