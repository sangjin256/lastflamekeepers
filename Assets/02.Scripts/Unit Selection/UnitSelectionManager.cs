using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : BehaviourSingleton<UnitSelectionManager>
{
    public List<GameObject> SelectedUnitList = new List<GameObject>();

    public bool IsDragging = false;

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        SelectedUnitList.Add(unitToAdd);

        SelectTest(unitToAdd);
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (!SelectedUnitList.Contains(unitToAdd))
        {
            SelectedUnitList.Add(unitToAdd);

            SelectTest(unitToAdd);
        }
        else
        {
            SelectedUnitList.Remove(unitToAdd);

            DeSelectTest(unitToAdd);
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if (!SelectedUnitList.Contains(unitToAdd))
        {
            SelectedUnitList.Add(unitToAdd);

            SelectTest(unitToAdd);
        }
    }

    public void DeselectAll()
    {
        DeselectAllTest();
        SelectedUnitList.Clear();
    }

    public void Deselect(GameObject unitToSelect)
    {
        SelectedUnitList.Remove(unitToSelect);

        DeSelectTest(unitToSelect);
    }

    public void SelectTest(GameObject unitSelect)
    {
        unitSelect.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void DeSelectTest(GameObject unitSelect)
    {
        unitSelect.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void DeselectAllTest()
    {
        foreach(GameObject unit in SelectedUnitList)
        {
            unit.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public bool GetIsDragging()
    {
        return IsDragging;
    }
}
