using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerScript : MonoBehaviour {
    float previousValue;
    float nextValue;
    bool FireLeft = false;
    bool FireRight = false;
    bool isFirstTime;
    System.Net.Sockets.UdpClient RollReceiver;
    System.Threading.Thread MyThread;
    System.Threading.Thread LeftSpinThread;
    System.Threading.Thread RightSpinThread;
    string received;
    byte[] receivedByteArray;
    Quaternion target;
    int FixedPoint;
    // Use this for initialization
    void LeftSpinner()
    {
        float TargetValue = transform.rotation.y - 5;
        while(true)
        {
            float InitialValue = transform.rotation.y;
            if(TargetValue==InitialValue)
            {
                break;
            }
            else
            {
                transform.Rotate(Vector3.left, 5);
            }
        }
    }
    void RightSpinner()
    {
        float TargetValue = transform.rotation.y + 5;
        while (true)
        {
            float InitialValue = transform.rotation.y;
            if (TargetValue == InitialValue)
            {
                break;
            }
            else
            {
                transform.Rotate(Vector3.right, 5);
            }
        }
    }
    void ThreadFunction()
    {
        FixedPoint = 180;
        RollReceiver = new System.Net.Sockets.UdpClient(4002);
        System.Net.IPEndPoint serverAddress = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
        while (true)
        {
            receivedByteArray = RollReceiver.Receive(ref serverAddress);
            received = System.Text.Encoding.ASCII.GetString(receivedByteArray);
            string[] values = received.Split('|');
            if (isFirstTime==true)
            {
                previousValue = 0;
                nextValue = 0;
                isFirstTime = false;
            }
            else
            {
                nextValue = float.Parse(values[0]);
                float Difference = nextValue - previousValue;
                if(Difference>=5)
                {
                    previousValue = nextValue;
                    FireRight = true;
                }
                else if(Difference<=-5)
                {
                    previousValue = nextValue;
                    FireLeft = true;
                }
            }
           
        }
    }
    void Start ()
    {
        MyThread = new System.Threading.Thread(ThreadFunction);
        MyThread.Start();
        LeftSpinThread = new System.Threading.Thread(LeftSpinner);
        RightSpinThread = new System.Threading.Thread(RightSpinner);

        isFirstTime = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(FireLeft==true)
        {
            transform.Rotate(Vector3.left, 5);
            //LeftSpinThread.Start();
            FireLeft = false;
        }   
        if(FireRight==true)
        {
            transform.Rotate(Vector3.right, 5);
            //RightSpinThread.Start();
            FireRight = false;
        }
        
    }
}
