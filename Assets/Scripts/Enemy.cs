using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject explodeFX;
    [SerializeField] Transform parent;
    ScoreDisplay scoreDisplay;
    int scoreValue = 10;

    void Start()
    {
        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        AddNonTriggerBoxCollider();
    }

    void OnParticleCollision(GameObject other)
    {
        //print("particles collided with game object " + gameObject.name);
        //Destroy(other);
        GameObject fx = Instantiate(explodeFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        scoreDisplay.Scored(scoreValue);
        Destroy(gameObject);
    }

    void AddNonTriggerBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }
}
