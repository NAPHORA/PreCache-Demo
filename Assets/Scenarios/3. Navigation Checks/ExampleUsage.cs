using Naphora.PreCache;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Unity.AI.Navigation;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ExampleUsage : MonoBehaviour, IPreCache
{
    public Transform Point1;
    public Transform Point2;

    public void PreCache()
    {
        //Collect all nav mesh surfaces that didn't awake (register self to nav system)
        var surfaces = GetComponentsInScene<NavMeshSurface>(gameObject.scene)
            .Where(x => x.didAwake is false)
            .ToArray();

        //Manually Register Nav Mesh Surfaces
        {
            var method = typeof(NavMeshSurface).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var surface in surfaces)
                method.Invoke(surface, null);
        }

        //Perform navigation sanity checks
        {
            if (CheckPathValid(Point1.position, Point2.position) is false)
                Debug.LogError("Path from Point1 to Point2 blocked");
            else
                Debug.Log("Path from Point1 to Point2 clear");
        }

        //Manually Unregister Nav Mesh Surfaces
        {
            var method = typeof(NavMeshSurface).GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var surface in surfaces)
                method.Invoke(surface, null);
        }
    }

    static IEnumerable<T> GetComponentsInScene<T>(Scene scene, bool findInactive = true) where T : class
    {
        if (!scene.IsValid())
        {
            return Enumerable.Empty<T>();
        }

        return scene.GetRootGameObjects().SelectMany((GameObject x) => x.GetComponentsInChildren<T>(findInactive));
    }

    static bool CheckPathValid(Vector3 start, Vector3 end)
    {
        var path = new NavMeshPath();
        var filter = new NavMeshQueryFilter()
        {
            agentTypeID = 0, //Always Humanoid
            areaMask = NavMesh.AllAreas,
        };

        if (NavMesh.CalculatePath(start, end, filter, path) is false)
            return false;

        return path.status is NavMeshPathStatus.PathComplete;
    }
}