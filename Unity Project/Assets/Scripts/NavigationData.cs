using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationData : MonoBehaviour {

    System.Net.Sockets.UdpClient DataReceiver; // Socket for data connection 
    System.Threading.Thread DataReceiver_Thread; // thread that receives data through data socket.
    System.Net.IPEndPoint serverAddress;
    //receiving these with numbering to split.
    string Drone_BatteryStatus;   //4
    string Drone_Pitch;           //1
    string Drone_Yaw;             //3
    string Drone_Roll;            //2
    string Drone_Altitude;        //5

    //GameObject Variables.
    public GameObject Battery_Text;
    public GameObject Altitude_Text;
    public GameObject Roll_Text;
    public GameObject Pitch_Text;
    public GameObject Yaw_Text;
    public GameObject ConnectionStatus;

    string Received;
    public float PitchFloat;
    public float BatteryFloat;
    // Use this for initialization
    void Start ()
    {
        DataReceiver = new System.Net.Sockets.UdpClient(4004);
        DataReceiver_Thread = new System.Threading.Thread(DataSplitter);
        serverAddress = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 4004);
        DataReceiver_Thread.Start();
        ConnectionStatus.GetComponent<TextMesh>().text = "Connection : Activated";
    }
	
    public void DataSplitter()
    {
        byte[] ReceivedByte_Array;
        string Received_String;
        string[] Spliting_Received ;
        while(true)
        {
            //Receiving Byte array and converting it to string.
            ReceivedByte_Array = DataReceiver.Receive(ref serverAddress);
            Received_String = System.Text.Encoding.ASCII.GetString(ReceivedByte_Array);
            Received = Received_String;
            //Spliting and setting equal to variables of Drone_ - - -
            Spliting_Received = Received_String.Split('|'); 
            Drone_Pitch = Spliting_Received[0]; //
            Drone_Roll = Spliting_Received[1];
            Drone_Yaw = Spliting_Received[2];
            Drone_BatteryStatus = Spliting_Received[3];
            Drone_Altitude = Spliting_Received[4];

            //misc
            BatteryFloat = float.Parse(Drone_BatteryStatus);
            PitchFloat = float.Parse(Drone_Pitch);

            //sleep value
            System.Threading.Thread.Sleep(100);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        //text values setting.
        Battery_Text.GetComponent<TextMesh>().text = "Battery : " + BatteryFloat.ToString();
        Altitude_Text.GetComponent<TextMesh>().text = "Altitude : " + Drone_Altitude;
        Roll_Text.GetComponent<TextMesh>().text = "Roll : " + Drone_Roll;
        Pitch_Text.GetComponent<TextMesh>().text = "Pitch : " + PitchFloat.ToString();
        Yaw_Text.GetComponent<TextMesh>().text = "Yaw : " + Drone_Yaw;
    }
}
