using Godot;
using System;

public class Sword : Node2D
{
    private AnimationPlayer SwordAnimationPlayer;
    private Hitbox SwordHitbox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SwordAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        SwordHitbox = GetNode<Hitbox>("Parts/Hitbox");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion mMotion)
        {
            this.Position = mMotion.GlobalPosition;
        }
        // If a mouse button is pressed
        if(@event is InputEventMouseButton mbEvent && mbEvent.IsPressed())
        {
            // And it's the left one
            if(mbEvent.ButtonIndex == (int)ButtonList.Left)
            {
                SwordHitbox.SetAttackFromVector(this.GlobalPosition - new Vector2(0,200));
                SwordAnimationPlayer.Play("SwordAttack");
            }
        }
    }
}
