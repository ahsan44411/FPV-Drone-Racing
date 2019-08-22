# FPV-Drone-Racing-using-Unity
We are using a three tier architecture for this project.  

# Objective: 
Our objective was to control any drone's movement using a mobile phone’s orientation and streaming live feed of the drone to give the user a first-person view of the drone. The video will be streamed on a VR view on the mobile phone. 

 

# Connections: 
Router: 
We used a router as a communication medium for faster data transfer and retrieval between the laptop and the mobile phone 

Wi-Fi Module:
We were using a WIFI module to connect the drone and the laptop 

 

# Video Streaming 
Drone to Laptop: 
We receive the video from the drone on our laptop using UDP connection. The video is received frame by frame and converted into JPEG format because is size is smaller than a frame. 

Laptop to mobile phone: 
The laptop sends the UDP packets to the phone. The code for the video receiver on phone is in unity. The video is displayed on the VR view of the phone. 

 

# Phone Orientation and Drone control: 
To get the phones orientation we used something called Quaternion in unity. The Eular angle of Quaternion gives the Yaw, Pitch and Roll for the phone. These reading are sent to the laptop using a UDP connection. The laptop handles the reading and sends commands to the drone. 

# Drone SDK: 
Documentation for the drone can be found here.  


# Limitation: 
The IP of the laptop in the unity app is hard coded (192.168.0.100) so every time you use the code, the laptop should be the first one to connect to the router and then the phone (IP: 192.168.0.101). Or you could rewrite the IP every time. 
