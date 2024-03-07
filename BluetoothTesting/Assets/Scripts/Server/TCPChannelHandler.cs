using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using shared;

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

    private FlashlightLogic flashlightLogic;
    private PlayerMovement playermove;
    

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
        flashlightLogic = FindObjectOfType<FlashlightLogic>();
        playermove = FindObjectOfType<PlayerMovement>();
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        ReceiveAndHandleTCPMessage();

        //Temporary code to actually return values while on PC
        if (channel != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SendString();
            }

            if (Input.GetMouseButtonDown(0))
            {
                SendGyroData();
            }

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


    //Handling of sending Data
    public void SendString(string message = "TEST MESSAGE")
    {
        ChatMessage msg = new ChatMessage();
        msg.message = message;
        SendData(msg);
    }

    public void SendGyroData(Quaternion gyroRotation = new Quaternion())
    {
        GyroData data1 = new GyroData();
        data1.gX = gyroRotation.x;
        data1.gY = gyroRotation.y;
        data1.gZ = gyroRotation.z;
        data1.gW = gyroRotation.w;
        SendData(data1);
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
                flashlightLogic.isIntensityChanged((message as LightData).luxValue);
                break;
            
            case JoystickData:
                Debug.Log((message as JoystickData).jX);
                Debug.Log((message as JoystickData).jY);

                playermove.SetMovementsDirection((message as JoystickData).jX, (message as JoystickData).jY);
                break;

            case LookData:
                Debug.Log((message as LookData).lX);
                Debug.Log((message as LookData).lY);

                playermove.SetLookDirection((message as LookData).lX, (message as LookData).lY);
                break;
            
            case InteractData:
                Debug.Log((message as InteractData).interactPressed);
                break;
        }
        
    }

}
