using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class videoReceiver : MonoBehaviour {
    TcpListener tcpclientforVideo;
    UdpClient udpClient;
    IPEndPoint serverAddr = new IPEndPoint(IPAddress.Any, 5000);
    public byte[] receivedByteArray;
    Thread receiveTCPThread;
    Plane plane;
    public TextMesh TM;
    string t;
    Texture2D texture;
    public GameObject TT;
    public static IPEndPoint serverAddress=null;
	// Use this for initialization
	void Start () {
        texture = new Texture2D(2, 2);
        receivedByteArray = new byte[40000];
        receiveTCPThread = new Thread(new ThreadStart(receiveVideoThread));
        receiveTCPThread.Priority = System.Threading.ThreadPriority.Highest;
        //receiveTCPThread.IsBackground = true;
        receiveTCPThread.Start();
    }

    // Update is called once per frame
    void Update () {
             texture.LoadImage(receivedByteArray);
            GetComponent<Renderer>().material.mainTexture = texture;
          

    }
    void receiveVideoThread()
   {

        udpClient = new UdpClient(5000);
        //byte[] ReceivedArray = new byte[40000];
        serverAddress = new IPEndPoint(IPAddress.Any, 0);
        int temp = 0;
        while (true)
        {
            receivedByteArray = udpClient.Receive(ref serverAddress);
            if(temp==0){
                t = serverAddress.Address.ToString();
                temp=1;
            }
            
        }
    }
   
    void OnApplicationQuit()
    {
        Debug.Log("Quitting");
        receiveTCPThread.Abort();
        if (udpClient != null)
        {
            udpClient.Close();
        }

    }
}