using Godot;
using System;
using System.Timers;

public class LoginScreen : Control
{
    NetworkedMultiplayerENet network = new NetworkedMultiplayerENet();
    String ip = "127.0.0.1";
    int port = 9001;
    Server server = new Server();
    System.Timers.Timer serverResponse = new System.Timers.Timer(1000);

    public void ConnectToServer()
    {
        network.CreateClient(ip, port);
        GetTree().NetworkPeer = network;

        network.Connect("connection_failed", this, "_OnConnectionFailed");
        network.Connect("connection_succeeded", this, "_OnConnectionSucceeded");
    }

    public void _OnConnectionFailed()
    {
        Console.WriteLine("Falha ao contectar-se com o servidor, provavelmente está Offline.");
    }

    public void _OnConnectionSucceeded()
    {
        Console.WriteLine("Conectado ao servidor com sucesso, provavelmente o servidor está Online.");
    }


    public override void _Ready()
    {
        //Singleton do servidor para realizar a comunicação via RPC (Client -> Server)
        server = GetNode<Server>("/root/Server");

        ConnectToServer();
        // var music = GetNode<AudioStreamMP3>("BGM");
        // music.Loop = true;
    }

    public void OnLoginButtonPressed()
    {
        string username = GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditUser").Text;
        byte[] encDataByte = new byte[GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text.Length];
        encDataByte = System.Text.Encoding.UTF8.GetBytes(GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text);
        var hashPassword = System.Security.Cryptography.SHA256.Create().ComputeHash(encDataByte);

        server.TryToLogin(username, hashPassword);
        
        serverResponse.Start();
    }

    public void OnRegisterButtonPressed()
    {
        string username = GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditUser").Text;
        string password = GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text;
        if (username != string.Empty && password != string.Empty)
        {
            byte[] encDataByte = new byte[password.Length];
            encDataByte = System.Text.Encoding.UTF8.GetBytes(password);
            var hashPassword = System.Security.Cryptography.SHA256.Create().ComputeHash(encDataByte);

            foreach (var button in this.GetChildren())
            {
                if (button.GetType() == typeof(Button))
                {
                    ((Button)button).Disabled = true;
                }
            }

            server.TryToRegister(username, hashPassword);

            foreach (var button in this.GetChildren())
            {
                if (button.GetType() == typeof(Button))
                {
                    ((Button)button).Disabled = false;
                }
            }
        }        
    }
}