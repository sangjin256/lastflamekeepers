using UnityEngine;

public class UnitClick : MonoBehaviour
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
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelectionManager.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelectionManager.Instance.ClickSelect(hit.collider.gameObject);
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
    }
}
