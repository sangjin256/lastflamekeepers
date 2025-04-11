using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UntDrag : MonoBehaviour
{
    public RectTransform BoxVisual;

    Rect SelectionBox;

    Vector2 StartPosition;
    Vector2 StartWorldPosition;
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
            StartPosition = Input.mousePosition;
            StartWorldPosition = Camera.main.ScreenToWorldPoint(StartPosition);
            SelectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if (SelectionBox.width >= 0.1f) UnitSelectionManager.Instance.SetDrag(true);

            SelectUnits();
            EndPosition = Input.mousePosition;
            DrawVisual(true);
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartPosition =  Vector2.zero;
            StartWorldPosition = Vector2.zero;
            EndPosition = Vector2.zero;
            DrawVisual(false);

            UnitSelectionManager.Instance.SetDrag(false);
        }
    }

    private void DrawVisual(bool isEnable)
    {
        Vector2 boxStart = (Vector2)Camera.main.WorldToScreenPoint(StartWorldPosition) - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        Vector2 boxEnd = EndPosition - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        if (isEnable == false)
        {
            boxStart = StartPosition;
            boxEnd = EndPosition;
        }

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        BoxVisual.anchoredPosition = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        BoxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        Vector2 startScreenPosition = Camera.main.WorldToScreenPoint(StartWorldPosition);
        Vector2 InputMousePosition = Input.mousePosition;
        if (InputMousePosition.x < startScreenPosition.x)
        {
            SelectionBox.xMin = InputMousePosition.x;
            SelectionBox.xMax = startScreenPosition.x;
        }
        else
        {
            SelectionBox.xMin = startScreenPosition.x;
            SelectionBox.xMax = InputMousePosition.x;
        }

        if (InputMousePosition.y < startScreenPosition.y)
        {
            SelectionBox.yMin = InputMousePosition.y;
            SelectionBox.yMax = startScreenPosition.y;
        }
        else
        {
            SelectionBox.yMin = startScreenPosition.y;
            SelectionBox.yMax = InputMousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (var unit in UnitManager.Instance.UnitList)
        {
            if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            // 테스트 코드
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
            // 드래그 할때랑 클릭할때랑 동시에 사용되서 1프레임 내에서 select deselect가 일어남
            else
            {
                if (UnitSelectionManager.Instance.GetIsDragging()) UnitSelectionManager.Instance.Deselect(unit);
            }
        }
    }
}
