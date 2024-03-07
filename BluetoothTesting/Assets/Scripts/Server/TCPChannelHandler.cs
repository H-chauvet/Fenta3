using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using shared;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Runtime.CompilerServices;


public class TCPChannelHandler : MonoBehaviour
{
    //Singleton for global access to public function without needing to make a refference
    private static TCPChannelHandler _instance;
    
    public static TCPChannelHandler Instance
    {
        get
        {
            return _instance;   
        }
        
    }
    
    public enum ClientType
    {
        Game,
        Phone
    }
    
    //Channel data
    public TcpMessageChannel channel { get; private set; }
    public string IP = "127.0.0.1";
    public int port = 55555;
    public ClientType clientType;
    private PlayerMovement playermove;

    public delegate void LightEvent(float luxValue);
    public LightEvent lightEvent;
    public delegate void JoystickEvent(float horizontal, float vertical);
    public JoystickEvent joystickEvent;
    public delegate void LookEvent(float horizontal, float vertical);
    public LookEvent lookEvent;
    public delegate void InteractEvent(bool interactPressed);
    public InteractEvent interactEvent;
    

    protected void Awake()
    {
        //Instantiate singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        channel = new TcpMessageChannel();

        //connect to the server
        channel.Connect(IP, port);

        PlayerJoinRequest joinRequest = new PlayerJoinRequest();
        joinRequest.name = clientType.ToString();
        channel.SendMessage(joinRequest);
        DontDestroyOnLoad(this);
    }
    

    void Update()
    {
        ReceiveAndHandleTCPMessage();

        //Temporary code to actually return values while on PC
        if (channel != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SendJoystickData();
                
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SendLightData();
            }
        }
    }


    public void SendLightData(float lightStrength = 69)
    {
        LightData lData = new LightData();
        lData.luxValue = lightStrength;
        SendData(lData);
    }

    public void SendJoystickData(Vector2 direction = new Vector2())
    {
        JoystickData joyData = new JoystickData();
        joyData.jX = direction.x;
        joyData.jY = direction.y;
        SendData(joyData);
    }
    
    //For ease of readability
    void SendData(ASerializable data)
    {
        channel.SendMessage(data);
    }
    
    //Handling of Receiving messages
    virtual protected void ReceiveAndHandleTCPMessage()
    {
        if (!channel.Connected)
        {
            Debug.LogWarning("Not connected to a server. Please run the server first and then connect");
            return;
        }

        //while there are messages, we have no issues AAAND we haven't been disabled (important!!):
        //we need to check for gameObject.activeSelf because after sending a message and switching state,
        //we might get an immediate reply from the server. If we don't add this, the wrong state will be processing the message
        while (channel.HasMessage() && gameObject.activeSelf)
        {
            ASerializable message = channel.ReceiveMessage();
            HandleNetworkMessage(message);
        }
    }

    public void HandleNetworkMessage(ASerializable message)
    {
        
        if (message == null) return;
        switch (message)
        {
            //TODO: Replace content with actual logic
            case ChatMessage:
                Debug.Log((message as ChatMessage).message);
                break;
            
            case GyroData:
                Debug.Log((message as GyroData).gX);
                Debug.Log((message as GyroData).gY);
                Debug.Log((message as GyroData).gZ);
                Debug.Log((message as GyroData).gW);
                break;
            
            case LightData:
                Debug.Log((message as LightData).luxValue);
                lightEvent?.Invoke((message as LightData).luxValue);
                break;
            
            case JoystickData:
                Debug.Log((message as JoystickData).jX);
                Debug.Log((message as JoystickData).jY);
                joystickEvent?.Invoke((message as JoystickData).jX, (message as JoystickData).jY);
                break;

            case LookData:
                Debug.Log((message as LookData).lX);
                Debug.Log((message as LookData).lY);
                lookEvent?.Invoke((message as LookData).lX, (message as LookData).lY);
                break;
            
            case InteractData:
                Debug.Log((message as InteractData).interactPressed);
                interactEvent?.Invoke((message as InteractData).interactPressed);
                break;
        }
        
    }

}
