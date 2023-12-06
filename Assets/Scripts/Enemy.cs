using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject explodeFX;
    [SerializeField] Transform parent;
    [SerializeField] int scoreValue = 10;
    [SerializeField] int hits = 10;
    ScoreDisplay scoreDisplay;

    void Start()
    {
        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        AddNonTriggerBoxCollider();
    }

    void OnParticleCollision(GameObject other)
    {
        //print("particles collided with game object " + gameObject.name);
        //Destroy(other);
        scoreDisplay.Scored(scoreValue);
        hits--;
        if (hits <= 0) Die();
    }

    void AddNonTriggerBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    void Die()
    {
        GameObject fx = Instantiate(explodeFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
