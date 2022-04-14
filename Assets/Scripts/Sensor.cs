using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Bullet"))
        {
            SimplePool.Despawn(other.gameObject);
        }
        else if (!other.gameObject.CompareTag("Sensor")) // All that is not a sensor
        {
            Destroy(other.gameObject);
        }
    }
}
