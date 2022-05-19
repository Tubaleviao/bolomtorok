using Godot;
using System;

public class UI : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

            public void OnRegisterButtonPressed()
        {
            Console.WriteLine("Deu certo (botão de Registro)");

            string username = GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditUser").Text;
            string password = GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text;
            if (GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditUser").Text != string.Empty &&
                GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text != string.Empty)
            {

                byte[] encDataByte = new byte[GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text.Length];
                encDataByte = System.Text.Encoding.UTF8.GetBytes(GetNode<LineEdit>("./BackGroundImage - Login Resource2/LineEditPassword").Text);
                var hashPassword = System.Security.Cryptography.SHA256.Create().ComputeHash(encDataByte);


                Console.WriteLine(this.GetParent().GetPath());
            }
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
