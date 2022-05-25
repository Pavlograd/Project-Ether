using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance;

    protected virtual void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this as T;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        instance = null;
        Destroy(gameObject);
    }
}

public abstract class PersistantSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
