using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {

            if (instance == null)
            {
                Debug.LogError("Singleton instance has not been created yet!");
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        this.LoadInstance();
    }

    private void LoadInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return;
        }

        if (instance != this)
        {
            Debug.LogError("Another instance of Singleton already exits!");
        }
    }
}
