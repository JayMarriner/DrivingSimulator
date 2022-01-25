using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
            GetComponentInParent<Driving>().collisionDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GetComponentInParent<Driving>().collisionDetected = false;
            GetComponentInParent<Driving>().ResetBrakes();
        }
    }
}
