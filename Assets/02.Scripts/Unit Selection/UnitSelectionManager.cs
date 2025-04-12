using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitSelectionManager : BehaviourSingleton<UnitSelectionManager>
{
    public List<Unit> SelectedUnitList = new List<Unit>();

    private bool IsDragging = false;
    private MaterialPropertyBlock _materialPropertyBlock;

    private void Start()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }
    public void ClickSelect(Unit unitToAdd)
    {
        DeselectAll();
        SelectedUnitList.Add(unitToAdd);

        SelectTest(unitToAdd);
    }

    public void ShiftClickSelect(Unit unitToAdd)
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

    public void DragSelect(Unit unitToAdd)
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

    public void Deselect(Unit unitToSelect)
    {
        SelectedUnitList.Remove(unitToSelect);

        DeSelectTest(unitToSelect);
    }

    public void SelectTest(Unit unitSelect)
    {
        //unitSelect.GetComponent<SpriteRenderer>().color = Color.red;
        //unitSelect.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 1);
        _materialPropertyBlock.SetFloat("_OutlineAlpha", 1);
        unitSelect.Renderer.SetPropertyBlock(_materialPropertyBlock);

    }
    public void DeSelectTest(Unit unitSelect)
    {
        //unitSelect.GetComponent<SpriteRenderer>().color = Color.white;
        //unitSelect.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0);
        _materialPropertyBlock.SetFloat("_OutlineAlpha", 0);
        unitSelect.Renderer.SetPropertyBlock(_materialPropertyBlock);
        Debug.Log("de");
    }

    public void DeselectAllTest()
    {
        _materialPropertyBlock.SetFloat("_OutlineAlpha", 0);
        foreach (Unit unit in SelectedUnitList)
        {
            //unit.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineAlpha", 0);
            unit.Renderer.SetPropertyBlock(_materialPropertyBlock);

        }
    }

    public void SetDrag(bool OnDrag)
    {
        if (OnDrag == false) StartCoroutine(LateDragOff());
        else IsDragging = OnDrag;
    }

    public IEnumerator LateDragOff()
    {
        yield return new WaitForSeconds(0.1f);
        IsDragging = false;
    }

    public bool GetIsDragging()
    {
        return IsDragging;
    }
}
