using Godot;
using CharacterStatus;
using System;

public class Player : KinematicBody
{
    Character.FacingDirection facingDirection = Character.FacingDirection.Down;
    Character.PoseStatus poseStatus = Character.PoseStatus.Idle;
    Vector3 initialPosition = new Vector3(0, 0, 0);
    Vector2 inputDirection = new Vector2(0, 0);
    AnimationTree animationTree = new AnimationTree();
    AnimationNodeStateMachinePlayback animationState = new AnimationNodeStateMachinePlayback();
    private float speed = 10.0f;
    private float gravity = 30.0f;
    private float jumpForce = 12.0f;
    private float velocity_y = 0.0f;
    bool isMoving = false;

    public override void _Ready()
    {
        if (GetNode("AnimationTree") != null)
        {
            animationTree = GetNode<AnimationTree>("AnimationTree");
            animationTree.Active = true;
            if (animationTree.Get("parameters/playback") != null)
            {
                animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
            }
        }

        initialPosition = GlobalTransform.origin;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (poseStatus == Character.PoseStatus.Turn)
            return;

        else if (!isMoving)
            ProcessPlayerInput();
        else if (inputDirection != Vector2.Zero)
        {
            animationState.Travel("Walk");
            Move(delta);
        }
        else
        {
            animationState.Travel("Idle");
            isMoving = false;
        }
    }

    public void ProcessPlayerInput()
    {
        if (inputDirection.y == 0)
        {
            int left = Input.IsActionPressed("ui_left") ? 1 : 0;
            int right = Input.IsActionPressed("ui_right") ? 1 : 0;
            if (!(left > 0 && right > 0))
                inputDirection.x = right - left;
            else
            { inputDirection.x = right - left; }
        }

        if (inputDirection.x == 0)
        {
            int up = Input.IsActionPressed("ui_up") ? 1 : 0;
            int down = Input.IsActionPressed("ui_down") ? 1 : 0;
            if (!(up > 0 && down > 0))
                inputDirection.y = down - up;
            else
            { inputDirection.y = down - up; }
        }

        if (inputDirection != Vector2.Zero)
        {
            animationTree.Set("parameters/Idle/blend_position", inputDirection);
            animationTree.Set("parameters/Walk/blend_position", inputDirection);
            animationTree.Set("parameters/Turn/blend_position", inputDirection);

            if (NeedToTurn())
            {
                poseStatus = Character.PoseStatus.Turn;
                animationState.Travel("Turn");
            }
            else
            {
                initialPosition = GlobalTransform.origin;
                isMoving = true;
            }
        }

        else
        {
            animationState.Travel("Idle");
        }
    }

    public void Move(float Delta)
    {
        MoveAndSlide(new Vector3(inputDirection.x * speed, 0, inputDirection.y * speed));

        isMoving = false;
    }

    public bool NeedToTurn()
    {
        Character.FacingDirection newFacingDirection = new Character.FacingDirection();
        if (inputDirection.x < 0)
            newFacingDirection = Character.FacingDirection.Left;
        else if (inputDirection.x < 0)
            newFacingDirection = Character.FacingDirection.Right;
        else if (inputDirection.y < 0)
            newFacingDirection = Character.FacingDirection.Up;
        else
            newFacingDirection = Character.FacingDirection.Down;

        if (facingDirection != newFacingDirection)
        {
            facingDirection = newFacingDirection;
            return true;
        }

        facingDirection = newFacingDirection;
        return false;
    }

    public void FinishedTurning()
    {
        poseStatus = Character.PoseStatus.Idle;
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("jump"))
        {
            velocity_y = jumpForce;
        }
    }

    public override void _Process(float delta)
    {

    }
}
