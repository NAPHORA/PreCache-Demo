using Naphora.PreCache;

using UnityEngine;
using UnityEngine.Assertions;

public class ExampleUsage : MonoBehaviour, IPreCache
{
    [SerializeField]
    Rigidbody Rigidbody;

    public void PreCache()
    {
        Assert.IsNotNull(Rigidbody);
    }
}