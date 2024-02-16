using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using shared;

public class TCPChannelHandler : MonoBehaviour
{
    public static TCPChannelHandler Instance
    {
        get;
        private set;
    }
    
    public enum ClientType
    {
        Game,
        Phone
    }

    public TcpMessageChannel channel { get; private set; }
    public string IP = "127.0.0.1";
    public int port = 55555;
    public ClientType clientType;
    

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        channel = new TcpMessageChannel();

        //connect to the server
        channel.Connect(IP, port);

        PlayerJoinRequest joinRequest = new PlayerJoinRequest();
        joinRequest.name = clientType.ToString();
        channel.SendMessage(joinRequest);
    }


    void Update()
    {
        ReceiveAndHandleTCPMessage();

        //This part isn't necessary, it's just here to show that sending and receiving messages is working.
        if (channel != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SendMessage();
            }

            if (Input.GetMouseButtonDown(0))
            {
                SendGyroData();
            }
        }
    }


    //Handling of Sending ChatMessage
    void SendMessage()
    {
        ChatMessage msg = new ChatMessage();
        msg.message = "TEST MESSAGE";
        channel.SendMessage(msg);
    }

    void SendGyroData()
    {
        GyroData data1 = new GyroData();
        data1.gW = Time.deltaTime;
        channel.SendMessage(data1);
    }

    //Handling of Receiving messages
    virtual protected void ReceiveAndHandleTCPMessage()
    {
        if (!channel.Connected)
        {
            Debug.LogWarning("Trying to receive network messages, but we are no longer connected.");
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
        //CODE FOR HANDLING NETWORK MESSAGE GOES HERE
        //Ideally you would split this into multiple functions too, but it's an if statement here because I'm lazy
        if (message == null) return;
        if (message is ChatMessage)
        {
            Debug.Log((message as ChatMessage).message);
        }
        if (message is GyroData)
        {
            Debug.Log((message as GyroData).gW);
        }
    }

}
