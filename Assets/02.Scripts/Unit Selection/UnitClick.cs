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

        if (Input.GetMouseButtonDown(1))
        {
            if(UnitSelectionManager.Instance.SelectedUnitList.Count != 0)
            {
                Vector3 WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(WorldMousePosition, Vector2.zero, 1f, Interactable);
                // 땅이면 좌표 전달
                if (hit.collider == null)
                {
                    // z가 -10인거 생각해야됨
                    Debug.Log(WorldMousePosition);
                    //UnitSelectionManager.Instance.SelectedUnitList?.ForEach(x => x.SetTarget(WorldMousePosition));
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("여기 채워야됨");
                }
                else if (hit.collider.CompareTag("Resource"))
                {
                    Debug.Log("여기 채워야됨");
                }
            }
        }
    }
}
