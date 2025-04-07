using UnityEngine;
using System.Collections;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    public float ScrollSpeed = 120.0f;
    public float Speed = 10.0f;
    public float MaxOrthographicSize = 15f;
    public float MinOrthographicSize = 5f;
    public Transform CameraTarget;

    private Camera MainCamera;

    private void Start()
    {
        CameraTarget = this.transform;
        MainCamera = Camera.main;
    }

    private void Update()
    {
        float scroll = - Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed * Time.deltaTime;

        if (MainCamera.orthographicSize < MinOrthographicSize && scroll < 0)
        {
            MainCamera.orthographicSize = MinOrthographicSize;
        }
        else if(MainCamera.orthographicSize >= MaxOrthographicSize && scroll > 0)
        {
            MainCamera.orthographicSize = MaxOrthographicSize;
        }
        else
        {
            MainCamera.orthographicSize += scroll;
        }


        // 키보드로 카메라 이동
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + Vector3.up * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + Vector3.left * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + Vector3.down * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + Vector3.right * Speed * Time.deltaTime;
        }

        // 마우스로 카메라 이동
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
