using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLevelLoader : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadFirstLevel", 2f);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }
}
