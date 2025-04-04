using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : BehaviourSingleton<UnitSelection>
{
    public List<GameObject> SelectedUnitList = new List<GameObject>();
    public void DragSelection(GameObject unitToAdd)
    {
        if (!SelectedUnitList.Contains(unitToAdd))
        {
            SelectedUnitList.Add(unitToAdd);
            unitToAdd.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
