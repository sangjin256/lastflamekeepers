using UnityEngine;

public class FireRangeController : MonoBehaviour
{
    [SerializeField]
    private float amount = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FireManager.Instance.ExpandFire(amount);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            FireManager.Instance.ReduceFire(amount);
        }
    }
}
