using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireVisual : MonoBehaviour
{
    Animator AnimController;
    private void Start()
    {
        AnimController = GetComponent<Animator>();
        FireManager.Instance.OnFireRangeChanged += SpriteChange;
    }

    public void SpriteChange()
    {
        float rangeRate = FireManager.Instance.GetRange() / FireManager.Instance.GetMaxRange();
        AnimController.SetFloat("RangeRate", rangeRate);
    }
}
