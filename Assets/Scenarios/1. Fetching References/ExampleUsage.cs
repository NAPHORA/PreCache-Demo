using Naphora.PreCache;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleUsage : MonoBehaviour, IPreCache
{
    [SerializeField] //Can also add [HideInInspector]
    ExampleReference[] AllReferences;

    [SerializeField] //Can also add [HideInInspector]
    ExampleReference SpecificReference;

    public void PreCache()
    {
        AllReferences = GetComponentsInScene<ExampleReference>(gameObject.scene).ToArray();

        SpecificReference = AllReferences.Single(x => x.ID == 42);
    }

    static T GetComponentInScene<T>(Scene scene, bool findInactive = true)
    {
        if (!scene.IsValid())
        {
            return default(T);
        }

        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        GameObject[] array = rootGameObjects;
        foreach (GameObject gameObject in array)
        {
            T componentInChildren = gameObject.GetComponentInChildren<T>(findInactive);
            if (componentInChildren != null)
            {
                return componentInChildren;
            }
        }

        return default(T);
    }
    static IEnumerable<T> GetComponentsInScene<T>(Scene scene, bool findInactive = true) where T : class
    {
        if (!scene.IsValid())
        {
            return Enumerable.Empty<T>();
        }

        return scene.GetRootGameObjects().SelectMany((GameObject x) => x.GetComponentsInChildren<T>(findInactive));
    }
}