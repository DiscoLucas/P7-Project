using UnityEngine;
/// <summary>
/// Basic Singleton.
/// Inherit from this class to make your class a singleton.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Singleton that persists through scenes.
/// </summary>
/// <typeparam name="T">The type of the singleton.</typeparam>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            onDuplicateInstanceDestroyed();
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        base.Awake();
    }

    /// <summary>
    /// Called when a duplicate instance of the singleton is destroyed.
    /// </summary>
    protected virtual void onDuplicateInstanceDestroyed()
    {
        return;
    }
}

