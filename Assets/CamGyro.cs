using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGyro : MonoBehaviour
{
    GameObject camParent;
    void Start()
    {
        // Se habilita el giróscopo del celular
        Input.gyro.enabled = true;
    }

    void Update()
    {
        // Se obtienen los ángulos de rotación del giróscopo. Se debe calibrar bien para observar cuál es cada eje en el teléfono
        this.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
    }
}
