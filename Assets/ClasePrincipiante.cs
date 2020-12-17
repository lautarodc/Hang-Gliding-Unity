using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Net.Sockets;
using System.Net;
using System.Globalization;



public class ClasePrincipiante : MonoBehaviour
{
    /*
     * Based on Windex's flycam script found here: http://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/
     */

    private float x;
    private float y;
    private float z;
    private float rotationSpeed;
    private float linearSpeed;

    public float mainSpeed = 14.0f; //regular speed
    public float shiftAdd = 70.0f; //multiplied by how long shift is held.  Basically running
    private float totalRun = 1.0f;
    private float speedMultiplier; // Angryboy: Used by Y axis to match the velocity on X/Z axis

    private float pitch, roll, yaw;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        x = 0.0f;
        y = 0.0f;
        z = 0.0f;
        rotationSpeed = 75.0f;

        // Creamos el cliente UDP
        int receiverPort = 1999;
        UdpClient receiver = new UdpClient(receiverPort);
        receiver.BeginReceive(DataReceived, receiver);
    }


    private void DataReceived(IAsyncResult ar)
    {

        UdpClient c = (UdpClient)ar.AsyncState;
        IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

        string packet = System.Text.Encoding.UTF8.GetString(receivedBytes, 0, 20);
        HandlePosition(packet);

        c.BeginReceive(DataReceived, ar.AsyncState);
    }

    void HandlePosition(string angle)
    {
        string[] position = angle.Split(',');
        yaw = float.Parse(position[0], CultureInfo.InvariantCulture);
        pitch = float.Parse(position[1], CultureInfo.InvariantCulture);
        roll = float.Parse(position[2], CultureInfo.InvariantCulture);
    }


    void Update()
    {

        //Control del Ala-delta con los valores del ESP8266

        //Cabeceo - Pitch

        x = pitch;

        if (Mathf.Abs(x) <= 5)
        {
            x = 0;
        }

        // Guiñalabeo - RollYaw

        if (roll < 0)
        {

            z = roll;
            if (z <= -15)
            {
                y += Time.deltaTime * rotationSpeed;
            }
        }
        else if (roll > 0)
        {
            z = roll;
        }
        if ((z <= -7.5) && (z > -15))
        {
            y += Time.deltaTime * 15;
        }
        if ((z <= -15) && (z > -30))
        {
            y += Time.deltaTime * 30;
        }
        if ((z <= -30) && (z > -40))
        {
            y += Time.deltaTime * 50;
        }
        if (z <= -40)
        {
            y += Time.deltaTime * rotationSpeed;
        }
        if ((z >= 7.5) && (z < 15))
        {
            y -= Time.deltaTime * 15;
        }
        if ((z >= 15) && (z < 30))
        {
            y -= Time.deltaTime * 30;
        }
        if ((z >= 30) && (z < 40))
        {
            y -= Time.deltaTime * 50;
        }
        if (z >= 40)
        {
            y -= Time.deltaTime * rotationSpeed;
        }
        z = Mathf.Clamp(z, -45, 45);
        x = Mathf.Clamp(x, -45, 90);

        transform.localRotation = Quaternion.Euler(x, y, z);


        Vector3 p = GetBaseInput();

        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        p = p * mainSpeed;
        speedMultiplier = mainSpeed * Time.deltaTime;

        p = p * Time.deltaTime;

        Vector3 newPosition = transform.position;
        transform.Translate(p);
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;


        //Criterio de entrada en pérdida
        if ((x <= -40) && (x > -45))
        {
            newPosition.y += -1.01f * speedMultiplier;
        }
        if (x <= -45)
        {
            newPosition.y += -2.0f * speedMultiplier;
        }


        //Criterio de bajada
        if (x >= 20)
        {
            newPosition.y += -1.01f * speedMultiplier;
        }

        transform.position = newPosition;
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        p_Velocity += new Vector3(0, 0, 1);
        return p_Velocity;
    }
}
