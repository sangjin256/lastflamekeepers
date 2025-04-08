using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshManager : BehaviourSingleton<NavMeshManager>
{
    public NavMeshSurface navMeshSurface;
    public void BakeSurface()
    {
        navMeshSurface.BuildNavMesh();
    }
}
