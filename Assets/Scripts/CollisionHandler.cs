using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("In Seconds")][SerializeField] float levelLoadDelay = 1f;
    [Tooltip("FX prefab on player")][SerializeField] GameObject explodeFX;
    [SerializeField] float destroyDelay = 0.2f;

    //void OnCollisionEnter(Collision collision)
    //{
    //print("character collided);
    //}

    void Start()
    {
        explodeFX.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.name);
        StartDeathProcess();
        if (explodeFX) explodeFX.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(6).gameObject.SetActive(false);
        Invoke("ReloadScene", levelLoadDelay);
    }

    void StartDeathProcess()
    {
        SendMessage("OnDeath");
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
