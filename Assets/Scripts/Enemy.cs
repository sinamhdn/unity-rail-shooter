using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject explodeFX;
    [SerializeField] Transform parent;

    void Start()
    {
        AddNonTriggerBoxCollider();
    }

    void OnParticleCollision(GameObject other)
    {
        //print("particles collided with game object " + gameObject.name);
        //Destroy(other);
        GameObject fx = Instantiate(explodeFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        Destroy(gameObject);
    }

    void AddNonTriggerBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }
}
