using UnityEngine;
using UnityEngine.EventSystems;

public class UntDrag : MonoBehaviour
{
    public RectTransform BoxVisual;

    Rect SelectionBox;

    Vector2 StartPosition;
    Vector2 EndPosition;

    private void Start()
    {
        StartPosition = Vector2.zero;
        EndPosition = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            SelectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            EndPosition = Input.mousePosition - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;
            DrawVisual();
        }
    }

    private void DrawVisual()
    {
        Vector2 boxStart = StartPosition;
        Vector2 boxEnd = EndPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        BoxVisual.anchoredPosition = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        BoxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        Vector2 InputMousePosition = Input.mousePosition - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        if (InputMousePosition.x < StartPosition.x)
        {
            SelectionBox.xMin = InputMousePosition.x;
            SelectionBox.xMax = StartPosition.x;
        }
        else
        {
            SelectionBox.xMin = StartPosition.x;
            SelectionBox.xMax = InputMousePosition.x;
        }

        if (InputMousePosition.y < StartPosition.y)
        {
            SelectionBox.yMin = InputMousePosition.y;
            SelectionBox.yMax = StartPosition.y;
        }
        else
        {
            SelectionBox.yMin = StartPosition.y;
            SelectionBox.yMax = InputMousePosition.y;
        }
    }

    private void SelectUnits()
    {
        //foreach (var unit in UnitManager.Instance.UnitList)
        //{
        //    if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
        //    {
        //        UnitSelection.Instance.DragSelection(unit.gameObject);
        //    }
        //    else
        //    {
        //        unit.GetComponent<SpriteRenderer>().color = Color.white;
        //    }
        //}
    }
}
