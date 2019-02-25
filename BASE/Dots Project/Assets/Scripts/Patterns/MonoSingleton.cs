using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    private bool _wasInited;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();

                    if (_instance == null)
                        Debug.LogError(string.Format("Problem during the creation of {0}", typeof(T)));
                    else
                        _instance.Initialize();
                }
                else
                    _instance.Initialize();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Initialize();
        }
        else if (_instance != this)
        {
            Debug.LogError(string.Format("Created more than one singleton of {0} ", typeof(T)));
            Destroy(this.gameObject);
        }
    }
    
    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void Initialize()
    {
        if (!_wasInited)
        {
            Init();
            _wasInited = true;
        }
    }

    protected virtual void Init() { }
}