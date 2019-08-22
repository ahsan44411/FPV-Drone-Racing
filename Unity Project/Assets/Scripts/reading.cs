using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;


public class reading : MonoBehaviour
{
    private Socket Mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    //UDPSocket MySocket = new UDPSocket();
    string address = "192.168.0.100";
    int port = 8000;
    int previousValue;
    int max = -10;
    int compass=0,compassX=0,compassZ=0;
    Quaternion q;
    System.Threading.Thread SenderThread;

    void CompassReading()
    {
        while (true)
        {
            System.Threading.Thread.Sleep(3000);

            //compass = (int)(((Input.gyro.attitude.y * 100f) + 360) % 360);
            byte[] data = Encoding.ASCII.GetBytes(compass.ToString()+","+compassX.ToString()+","+compassZ.ToString());
            Mysocket.Send(data, data.Length, SocketFlags.None);


        }

    }

    // Use this for initialization
    void Start()
    {

        if (Input.gyro.enabled == false)
        {
            Input.gyro.enabled = true;
        }
        Input.gyro.updateInterval = 0.01f;
        previousValue = (int)(((Input.gyro.attitude.y * 100f) + 360) % 360);
        compass = previousValue;
        SenderThread = new System.Threading.Thread(new System.Threading.ThreadStart(CompassReading));
        SenderThread.Priority = System.Threading.ThreadPriority.Highest;
        SenderThread.Start();
        Mysocket.Connect(IPAddress.Parse(address), port);


    }

    // Update is called once per frame
    void Update()
    {
        /*int truedegree = (int)Input.compass.trueHeading;
        value = truedegree;*/
        q = Quaternion.Euler(90.0f, 0.0f, 0.0f) * new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        compass = (int)(q.eulerAngles.y);
        compassX = (int)(q.eulerAngles.x);
        compassZ = (int)(q.eulerAngles.z);
        GetComponent<TextMesh>().text = "Y = "+compass.ToString()+" , "+ "X = "+compassX.ToString()+" , "+"Z = "+compassZ.ToString();

    }
    void OnApplicationQuit()
    {
        SenderThread.Abort();
    }
}