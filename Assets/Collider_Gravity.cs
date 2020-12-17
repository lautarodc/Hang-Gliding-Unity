using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_Gravity : MonoBehaviour
{
    // Callback para la colisión, se activa la gravedad que por defecto está desactivada.    
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody delta = gameObject.GetComponent<Rigidbody>();
        delta.useGravity = true;
    }
}
