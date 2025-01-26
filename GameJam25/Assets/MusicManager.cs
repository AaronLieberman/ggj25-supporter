using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
