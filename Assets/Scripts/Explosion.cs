using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        if (!gameObject.GetComponentInParent<Player>()) Destroy(gameObject, 5f);
    }
}
