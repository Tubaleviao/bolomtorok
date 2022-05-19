using Godot;
using System;

public class Gateway : Node
{
    NetworkedMultiplayerENet network = new NetworkedMultiplayerENet();
    MultiplayerAPI gatewayApi = new MultiplayerAPI();
    string ip = "127.0.0.1";
    int port = 6902;

    string username;
    string password;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        if (CustomMultiplayer == null)
            return;
        if (!(CustomMultiplayer.NetworkPeer == null))
            return;
        CustomMultiplayer.Poll();
    }

    public void ConnectToServer(string _username, string _password)
    {
        network.CreateClient(ip, port);
        CustomMultiplayer = gatewayApi;
        CustomMultiplayer.RootNode = this;
        CustomMultiplayer.NetworkPeer = network;

        // network.Connect("peerConnected", this, "_PeerConnected");
        // network.Connect("peerDisconnected", this, "_PeerDisconnected");
        network.Connect("connectionFailed", this, "_OnConnectionFailed");
        network.Connect("connectionSucceeded", this, "_OnConnectionSucceeded");
    }

    public void _OnConnectionFailed()
    {
        Console.WriteLine("Failed to connect to login server");
        Console.WriteLine("Pop-up server offline or something");
        //TODO: Voltar pra LoginScreen
        //GetNode("/root/SceneHandler/GUI/LoginScreen").loginButton.disabled;
    }

    public void _OnConnectionSucceeded()
    {
        Console.WriteLine("Succesfully connected to Login Server");
        RequestLogin();
    }

    // public void _PeerConnected(int playerId)
    // {
    //     Console.WriteLine("User " + playerId + " Connected");

    // }

    // public void _PeerDisconnected(int playerId)
    // {
    //     Console.WriteLine("User " + playerId + " Disconnected");

    // }

    public void RequestLogin()
    {

    }
}
