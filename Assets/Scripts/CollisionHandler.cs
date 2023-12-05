using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    //void OnCollisionEnter(Collision collision)
    //{

    //}

    void OnTriggerEnter(Collider other)
    {
        StartDeathProcess();
    }

    void StartDeathProcess()
    {
        SendMessage("OnDeath");
    }
}
