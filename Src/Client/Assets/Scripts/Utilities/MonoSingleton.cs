using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    private void Awake()
    {
        if (global)
        {
            if (instance != this.gameObject.GetComponent<T>() && instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this.GetComponent<T>();
        }
    }

    void Start()
    {
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}