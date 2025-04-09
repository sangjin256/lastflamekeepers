using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    public float ScrollSpeed = 120.0f;
    public float Speed = 15.0f;
    public float MaxOrthographicSize = 15f;
    public float MinOrthographicSize = 3f;
    public Vector3 MousePosition;

    public Transform CameraTarget;

    public float SmoothTime = 0.12f;
    public Vector3 Velocity;

    private Camera MainCamera;
    private Vector3 TargetPosition;

    private void Start()
    {
        CameraTarget = this.transform;
        MainCamera = Camera.main;
        TargetPosition = this.transform.position;
    }

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float targetSize = MainCamera.orthographicSize - scrollInput * ScrollSpeed * Time.deltaTime;
            MainCamera.orthographicSize = Mathf.Clamp(targetSize, MinOrthographicSize, MaxOrthographicSize);
        }

        MousePosition = Input.mousePosition;

        Vector3 viewPortMousePosition = MainCamera.ScreenToViewportPoint(MousePosition);
        Vector3 movePosition = Vector3.zero;

        // 카메라 이동
        if (viewPortMousePosition.y > 0.98f || Input.GetKey(KeyCode.W)) movePosition += Vector3.up;
        if (viewPortMousePosition.x < 0.02f || Input.GetKey(KeyCode.A)) movePosition += Vector3.left;
        if (viewPortMousePosition.y < 0.02f || Input.GetKey(KeyCode.S)) movePosition += Vector3.down;
        if (viewPortMousePosition.x > 0.98f || Input.GetKey(KeyCode.D)) movePosition += Vector3.right;

        if (movePosition != Vector3.zero)
        {
            Vector3 nextPosition = transform.position + movePosition.normalized;
            if (Global.Instance.BoundaryCheck(nextPosition))
            {
                TargetPosition = nextPosition;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, SmoothTime, Speed);
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
