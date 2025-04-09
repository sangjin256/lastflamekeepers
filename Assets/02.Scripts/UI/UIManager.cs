using UnityEngine;

public class UIManager : BehaviourSingleton<UIManager>
{
    public GameObject FireObject;

    public void OnClickFire()
    {
        FireObject.SetActive(true);
    }
}
