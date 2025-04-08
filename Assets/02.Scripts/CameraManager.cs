using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    public float ScrollSpeed = 120.0f;
    public float Speed = 10.0f;
    public float MaxOrthographicSize = 15f;
    public float MinOrthographicSize = 5f;
    public Vector3 MousePosition;

    public Transform CameraTarget;

    private Camera MainCamera;

    private void Start()
    {
        CameraTarget = this.transform;
        MainCamera = Camera.main;
    }

    private void Update()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed * Time.deltaTime;

        if (MainCamera.orthographicSize < MinOrthographicSize && scroll < 0)
        {
            MainCamera.orthographicSize = MinOrthographicSize;
        }
        else if (MainCamera.orthographicSize >= MaxOrthographicSize && scroll > 0)
        {
            MainCamera.orthographicSize = MaxOrthographicSize;
        }
        else
        {
            MainCamera.orthographicSize += scroll;
        }

        MousePosition = Input.mousePosition;

        Vector3 viewPortMousePosition = MainCamera.ScreenToViewportPoint(MousePosition);
        Vector3 movePosition; 

        // 카메라 이동
        if (viewPortMousePosition.y > 0.98f || Input.GetKey(KeyCode.W))
        {
            movePosition = transform.position + Vector3.up;
            if (Global.Instance.BoundaryCheck(movePosition))
            {
                transform.position = Vector3.Lerp(transform.position, movePosition, Speed * Time.deltaTime);
            }
        }
        if (viewPortMousePosition.x < 0.02f || Input.GetKey(KeyCode.A))
        {
            movePosition = transform.position + Vector3.left;
            if (Global.Instance.BoundaryCheck(movePosition))
            {
                transform.position = Vector3.Lerp(transform.position, movePosition, Speed * Time.deltaTime);
            }
        }
        if (viewPortMousePosition.y < 0.02f || Input.GetKey(KeyCode.S))
        {
            movePosition = transform.position + Vector3.down;
            if (Global.Instance.BoundaryCheck(movePosition))
            {
                transform.position = Vector3.Lerp(transform.position, movePosition, Speed * Time.deltaTime);
            }
        }
        if (viewPortMousePosition.x > 0.98f || Input.GetKey(KeyCode.D))
        {
            movePosition = transform.position + Vector3.right;
            if (Global.Instance.BoundaryCheck(movePosition))
            {
                transform.position = Vector3.Lerp(transform.position, movePosition, Speed * Time.deltaTime);
            }
        }
    }

    public void MoveToEntity(Transform targetTransform)
    {
        StartCoroutine(MoveCameraToEnemy(targetTransform));
    }

    public IEnumerator MoveCameraToEnemy(Transform targetTransform)
    {
        Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y, -10f);
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Speed * Time.deltaTime);
            MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, MinOrthographicSize, Speed * Time.deltaTime);
            yield return null;
        }
    }
}
