using UnityEngine;
using System;
using System.Collections;

public class Global : BehaviourSingleton<Global>
{
    public Action OnDataLoaded;

    private IEnumerator Start()
    {
        yield return DataTable.Instance.Load_Routine();
        OnDataLoaded?.Invoke();
    }
}
