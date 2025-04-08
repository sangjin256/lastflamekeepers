using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : BehaviourSingleton<UnitSelectionManager>
{
    public List<Unit> SelectedUnitList = new List<Unit>();

    private bool IsDragging = false;

    public void ClickSelect(Unit unitToAdd)
    {
        DeselectAll();
        SelectedUnitList.Add(unitToAdd);

        SelectTest(unitToAdd.gameObject);
    }

    public void ShiftClickSelect(Unit unitToAdd)
    {
        if (!SelectedUnitList.Contains(unitToAdd))
        {
            SelectedUnitList.Add(unitToAdd);

            SelectTest(unitToAdd.gameObject);
        }
        else
        {
            SelectedUnitList.Remove(unitToAdd);

            DeSelectTest(unitToAdd.gameObject);
        }
    }

    public void DragSelect(Unit unitToAdd)
    {
        if (!SelectedUnitList.Contains(unitToAdd))
        {
            SelectedUnitList.Add(unitToAdd);

            SelectTest(unitToAdd.gameObject);
        }
    }

    public void DeselectAll()
    {
        DeselectAllTest();
        SelectedUnitList.Clear();
    }

    public void Deselect(Unit unitToSelect)
    {
        SelectedUnitList.Remove(unitToSelect);

        DeSelectTest(unitToSelect.gameObject);
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
        foreach(Unit unit in SelectedUnitList)
        {
            unit.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void SetDrag(bool OnDrag)
    {
        IsDragging = OnDrag;
    }

    public bool GetIsDragging()
    {
        return IsDragging;
    }
}
