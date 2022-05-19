using Godot;
using System;

public class Server : Node
{
    NetworkedMultiplayerENet network = new NetworkedMultiplayerENet();
    String ip = "127.0.0.1";
    int port = 9001;
    public bool logged = false;

    public override void _Ready()
    {
        ConnectToServer();
    }

    public override void _Process(float delta)
    {
        if (CustomMultiplayer == null)
            return;
        if (!(CustomMultiplayer.NetworkPeer == null))
            return;
        CustomMultiplayer.Poll();
    }

    public void ConnectToServer()
    {
        network.CreateClient(ip, port);
        GetTree().NetworkPeer = network;

        network.Connect("connection_failed", this, "_OnConnectionFailed");
        network.Connect("connection_succeeded", this, "_OnConnectionSucceeded");
    }

    public void _OnConnectionFailed()
    {
        Console.WriteLine("Failed to connect");
    }

    public void _OnConnectionSucceeded()
    {
        Console.WriteLine("Successfully connected");
    }

    public void TryToRegister(string username, byte[] hashPassword)
    {
        Rpc("ProcessRegister", username, hashPassword);
    }

    public void TryToLogin(string username, byte[] hashPassword)
    {
        try
        {            
            Console.WriteLine("Realizando tentativa de login");
            RpcId(1, "ProcessLogin", username, hashPassword);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    [Remote]
    public void LoginAccepted()
    {
        var scene = GetNode("../root/Scenes/LoginScreen2/UI");
        if (scene != null)
        {
            // scene.
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}