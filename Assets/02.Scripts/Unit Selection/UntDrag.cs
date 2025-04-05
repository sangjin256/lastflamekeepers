using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

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
            StartPosition = Input.mousePosition;
            SelectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if (SelectionBox.width != 0) UnitSelectionManager.Instance.IsDragging = true;

            SelectUnits();
            EndPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;
            DrawVisual();

            UnitSelectionManager.Instance.IsDragging = false;
        }
    }

    private void DrawVisual()
    {
        Vector2 boxStart = StartPosition - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        Vector2 boxEnd = EndPosition - new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        BoxVisual.anchoredPosition = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        BoxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        Vector2 InputMousePosition = Input.mousePosition;
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
        foreach (var unit in UnitManager.Instance.UnitList)
        {
            if (SelectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            // 테스트 코드
            {
                UnitSelectionManager.Instance.DragSelect(unit.gameObject);
            }
            // 드래그 할때랑 클릭할때랑 동시에 사용되서 1프레임 내에서 select deselect가 일어남
            else
            {
                if (UnitSelectionManager.Instance.GetIsDragging()) UnitSelectionManager.Instance.Deselect(unit.gameObject);
            }
        }
    }
}
