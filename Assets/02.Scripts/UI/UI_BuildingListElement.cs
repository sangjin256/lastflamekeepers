using UnityEngine;

public class UI_BuildingListElement : MonoBehaviour
{
    private ABaseBuilding _building;
    public ABaseBuilding Building => _building;

    public void SetBuilding(ABaseBuilding building)
    {
        _building = building;
    }
    public void Refresh()
    {

    }
}
