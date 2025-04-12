using UnityEngine;
using System.Collections;

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

    private Transform FollowTarget;
    private bool IsFollowing = false;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    private void Start()
    {
        CameraTarget = this.transform;
        MainCamera = Camera.main;
        TargetPosition = this.transform.position;
    }

    private void Update()
    {
        if (GameManager.Instance.IsCreatingUnit()) return;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float targetSize = MainCamera.orthographicSize - scrollInput * ScrollSpeed * Time.unscaledDeltaTime;
            MainCamera.orthographicSize = Mathf.Clamp(targetSize, MinOrthographicSize, MaxOrthographicSize);
        }

        MousePosition = Input.mousePosition;

        Vector3 viewPortMousePosition = MainCamera.ScreenToViewportPoint(MousePosition);
        Vector3 movePosition = Vector3.zero;

        if (viewPortMousePosition.y > 0.98f || Input.GetKey(KeyCode.W))
        {
            movePosition += Vector3.up;

            FollowTarget = null;
            IsFollowing = false;
        }
        if (viewPortMousePosition.x < 0.02f || Input.GetKey(KeyCode.A))
        {
            movePosition += Vector3.left;

            FollowTarget = null;
            IsFollowing = false;
        }
        if (viewPortMousePosition.y < 0.02f || Input.GetKey(KeyCode.S))
        {
            movePosition += Vector3.down;

            FollowTarget = null;
            IsFollowing = false;
        }
        if (viewPortMousePosition.x > 0.98f || Input.GetKey(KeyCode.D))
        {
            movePosition += Vector3.right;

            FollowTarget = null;
            IsFollowing = false;
        }

        MiddleMouseAction();

        if (isDragging == false)
        {
            if(movePosition != Vector3.zero)
            {
                Vector3 nextPosition = transform.position + movePosition.normalized;
                if (Global.Instance.BoundaryCheck(nextPosition))
                {
                    TargetPosition = nextPosition;
                }
            }
        }

        if (IsFollowing)
        {
            if (FollowTarget.Equals(null))
            {
                IsFollowing = false;
                FollowTarget = null;
            }
            else
            {

                Vector3 targetPos = new Vector3(FollowTarget.position.x, FollowTarget.position.y, -10f);
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref Velocity, SmoothTime, Speed, Time.unscaledDeltaTime);
            }
        }
        else transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, SmoothTime, Speed, Time.unscaledDeltaTime);
    }

    public void MoveToEntity(Transform targetTransform)
    {
        FollowTarget = targetTransform;
        IsFollowing = true;
    }

    public void MiddleMouseAction()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            dragOrigin = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            IsFollowing = false;
        }

        // 휠 클릭 중 이동
        if (Input.GetMouseButton(2) && isDragging)
        {
            Vector3 currentPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = dragOrigin - currentPos;
            Vector3 nextPosition = transform.position + diff;
            if (Global.Instance.BoundaryCheck(nextPosition))
            {
                TargetPosition = nextPosition;
                transform.position = TargetPosition;
            }
        }

        // 휠 클릭 해제
        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }
    }

    // 카메라 쉐이킹
    private Coroutine _cameraShakeCoroutine;
    public void CameraShake(float magnitude, float duration)
    {
        if (_cameraShakeCoroutine != null)
        {
            StopCoroutine(_cameraShakeCoroutine);
        }
        _cameraShakeCoroutine = StartCoroutine(ShakeCoroutine(magnitude, duration));
    }

    private IEnumerator ShakeCoroutine(float magnitude, float duration)
    {
        float elapsed = 0.0f;

        Vector3 originalPosition = transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
