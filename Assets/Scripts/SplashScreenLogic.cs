using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenLogic : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Invoke("LoadFirstLevel", 2f);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }
}
