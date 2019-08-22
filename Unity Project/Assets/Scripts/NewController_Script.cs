using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
public class NewController_Script : MonoBehaviour
{

    
    string XButton;
    string YButton;
    string AButton;
    string BButton;
    string RightHorizontal;
    string RightVertical;
    string LeftVertical;
    string LeftHorizontal;
    string L2R2Button;
    string L1Button;
    string R1Button;

    string previousCommand;
    System.Threading.Thread SenderThread;
    private Socket Mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    string address = "";
    int port = 4000;
    bool buttonCommandGiven = false;
    string buttonCommand;
    string leftJoyCommand;
    string rightJoyCommand;
    string finalCommand;
    int compass;
    Quaternion Angles;
    bool leftJoyCommandGiven = false;
    bool rightJoyCommandGiven = false;
    string temporaryCommand;
    string[] CurrentArray;
    string LRBcommand;
    bool isAutoSpinMode;
    int temTp = 0;
    public TextMesh TTTT;
    // Use this for initialization
    void SendReadings()
    {
        while (true)
        {   
            Thread.Sleep(50);
            if(videoReceiver.serverAddress!=null && temTp==0)
            {
                address = videoReceiver.serverAddress.Address.ToString();
                temTp = 1;
                Mysocket.Connect(IPAddress.Parse(address), port);
            }
            else
            {
                
                if (LRBcommand != previousCommand)
                {
                    byte[] data = Encoding.ASCII.GetBytes(finalCommand);
                    Mysocket.Send(data, data.Length, SocketFlags.None);
                    previousCommand = LRBcommand;
                    finalCommand = "";
                    temporaryCommand = "";
                }
                else if(isAutoSpinMode)
                {
                    byte[] data = Encoding.ASCII.GetBytes("-|"+compass);
                    Mysocket.Send(data, data.Length, SocketFlags.None);
                    previousCommand = LRBcommand;
                    finalCommand = "";
                    temporaryCommand = "";
                }
            }
        }
    }
    void checkControllerCommand()
    {

        XButton = Input.GetAxis("XButton").ToString();
        YButton = Input.GetAxis("YButton").ToString();
        AButton = Input.GetAxis("AButton").ToString();
        BButton = Input.GetAxis("BButton").ToString();
        RightHorizontal = Input.GetAxis("RightJoystickHorizontal").ToString();
        RightVertical = Input.GetAxis("RightJoystickVertical").ToString();
        LeftVertical = Input.GetAxis("LeftJoystickVertical").ToString();
        LeftHorizontal = Input.GetAxis("LeftJoystickHorizontal").ToString();
        L2R2Button = Input.GetAxis("L2R2Button").ToString();
        L1Button = Input.GetAxis("L1Button").ToString();
        R1Button = Input.GetAxis("R1Button").ToString();

        //All Buttons...
        if (XButton.Trim() == "1")
        {
            if (!buttonCommandGiven)
            {
                buttonCommand = "spintoggle";
                buttonCommandGiven = true;
                if (!isAutoSpinMode)
                {
                    isAutoSpinMode = true;
                }
                else
                {
                    isAutoSpinMode = false;
                }
            }
        }
        else if (YButton.Trim() == "1")
        {
            if (!buttonCommandGiven)
            {
                buttonCommand = "takeoff";
                buttonCommandGiven = true;
            }
        }
        else if (AButton.Trim() == "1")
        {
            if (!buttonCommandGiven)
            {
                buttonCommand = "land";
                buttonCommandGiven = true;
            }
        }
        else if (BButton.Trim() == "1")
        {
            if (!buttonCommandGiven)
            {
                buttonCommand = "hover";
                buttonCommandGiven = true;
            }
        }


        else
        {

            if (LeftHorizontal.Trim() == "1" && leftJoyCommandGiven == false)
            {
                leftJoyCommand = "right";
                leftJoyCommandGiven = true;
            }
            else if (LeftHorizontal.Trim() == "-1" && leftJoyCommandGiven == false)
            {
                leftJoyCommand = "left";
                leftJoyCommandGiven = true;
            }
            else if (LeftVertical.Trim() == "-1" && leftJoyCommandGiven == false)
            {
                leftJoyCommand = "forward";
                leftJoyCommandGiven = true;
            }
            else if (LeftVertical.Trim() == "1" && leftJoyCommandGiven == false)
            {
                leftJoyCommand = "backward";
                leftJoyCommandGiven = true;
            }


            if (RightHorizontal.Trim() == "1" && rightJoyCommandGiven == false && !isAutoSpinMode)
            {
                rightJoyCommand = "spinright";
                rightJoyCommandGiven = true;
            }
            else if (RightHorizontal.Trim() == "-1" && rightJoyCommandGiven == false && !isAutoSpinMode)
            {
                rightJoyCommand = "spinleft";

                rightJoyCommandGiven = true;
            }
            else if (RightVertical.Trim() == "1" && rightJoyCommandGiven == false)
            {
                rightJoyCommand = "up";

                rightJoyCommandGiven = true;
            }
            else if (RightVertical.Trim() == "-1" && rightJoyCommandGiven == false)
            {
                rightJoyCommand = "down";

                rightJoyCommandGiven = true;
            }
        }



        if (buttonCommandGiven)
        {
            finalCommand = buttonCommand+"|-";
            LRBcommand = buttonCommand;
        }
        else if (leftJoyCommandGiven && rightJoyCommandGiven)
        {
            temporaryCommand = leftJoyCommand + "," + rightJoyCommand;

            if (isAutoSpinMode)
            {
                finalCommand = temporaryCommand + "|" + compass;
            }
            else
            {
                finalCommand = temporaryCommand + "|-";
            }

            LRBcommand = temporaryCommand;

        }
        else if (leftJoyCommandGiven)
        {
            temporaryCommand = leftJoyCommand;

            if (isAutoSpinMode)
            {
                finalCommand = temporaryCommand + "|" + compass;
            }
            else
            {
                finalCommand = temporaryCommand + "|-";
            }
            LRBcommand = temporaryCommand;

        }
        else if (rightJoyCommandGiven)
        {
            temporaryCommand = rightJoyCommand;

            if (isAutoSpinMode)
            {
                finalCommand = temporaryCommand + "|" + compass;
            }
            else
            {
                finalCommand = temporaryCommand + "|-";
            }
            LRBcommand = temporaryCommand;

        }
        else
        {
            if (isAutoSpinMode)
            {
                finalCommand = "hoverexceptspin|" + compass;
                LRBcommand = "hoverexceptspin|";
            }
            else
            {
                finalCommand = "hover|-";
                LRBcommand = "hover|";

            }


        }



    }
    void checkRelease()
    {
        if (XButton.Trim() == "0" && YButton.Trim() == "0" && AButton.Trim() == "0" && BButton.Trim() == "0")
        {
            buttonCommandGiven = false;
            buttonCommand = "";
        }
        if (LeftHorizontal.Trim() != "-1" && LeftHorizontal.Trim() != "1"
        && LeftVertical.Trim() != "-1" && LeftVertical.Trim() != "1")
        {
            leftJoyCommandGiven = false;
            leftJoyCommand = "";
        }
        if (RightHorizontal.Trim() != "-1" && RightHorizontal.Trim() != "1"
        && RightVertical.Trim() != "-1" && RightVertical.Trim() != "1")
        {
            rightJoyCommandGiven = false;
            rightJoyCommand = "";
        }
        



    }
    void Start()
    {
        
        if (Input.gyro.enabled == false)
        {
            Input.gyro.enabled = true;
        }
        isAutoSpinMode = false;
        Input.gyro.updateInterval = 0.01f;
        previousCommand = "";
        CurrentArray = new string[2];
        buttonCommand = "-";
        LRBcommand = "";
        temporaryCommand = "";
        
        SenderThread = new System.Threading.Thread(new System.Threading.ThreadStart(SendReadings));
        //SenderThread.IsBackground = true;
        SenderThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Angles = Quaternion.Euler(90.0f, 0.0f, 0.0f) * new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        compass = (int)(Angles.eulerAngles.y);
        checkControllerCommand();
        checkRelease();
        GetComponent<TextMesh>().text = "Command : " + finalCommand;
        TTTT.GetComponent<TextMesh>().text = "GT : "+videoReceiver.serverAddress.Address.ToString();
    }



}