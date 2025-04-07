using UnityEngine;

public class ClickSystem : MonoBehaviour
{
    public LayerMask Interactable;
    public LayerMask Ground;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, Interactable);
            if(hit.collider != null)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    Unit unit = hit.collider.GetComponent<Unit>();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelectionManager.Instance.ShiftClickSelect(unit);
                    }
                    else
                    {
                        if (UnitSelectionManager.Instance.SelectedUnitList.Contains(unit))
                        {
                            // À¯´Ö ´õºíÅ¬¸¯
                            CameraManager.Instance.MoveToEntity(unit.transform);
                        }
                        UnitSelectionManager.Instance.ClickSelect(unit);
                    }
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if(UnitSelectionManager.Instance.GetIsDragging() == false) UnitSelectionManager.Instance.DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(UnitSelectionManager.Instance.SelectedUnitList.Count != 0)
            {
                Vector3 WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(WorldMousePosition, Vector2.zero, 1f, Interactable);
                // ¶¥ÀÌ¸é ÁÂÇ¥ Àü´Þ
                if (hit.collider == null)
                {
                    UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(WorldMousePosition));
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    //UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(hit.collider.GetComponent<AInteractableEntityTest>()));
                    Debug.Log("¿©±â Ã¤¿ö¾ßµÊ");
                }
                else if (hit.collider.CompareTag("Resource"))
                {
                    Debug.Log("¿©±â Ã¤¿ö¾ßµÊ");
                }
            }
        }
    }
}
