using Godot;
using System;

public class SpikedFly : KinematicBody2D, ITakeDamage
{

    [Export]
    public float Health = 10.0f;
    [Export]
    public float SpikedFlySpeed = 200;
    private Timer TakeDamageTimer;
    private Vector2 Velocity = new Vector2(-100,0);
    private Vector2 KnockBack = Vector2.Zero;
    private Label HealthPoints;
    private AnimatedSprite CloudDeathEffect;
    private bool Dead = false;
    private int lastCloudDeathFrame;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TakeDamageTimer = GetNode<Timer>("TakeDamageTimer");
        HealthPoints = GetNode<Label>("HealthPoints/HealthLabel");
        CloudDeathEffect = GetNode<AnimatedSprite>("CloudDeathEffect");
        HealthPoints.Text = "HP:" + Health;
        lastCloudDeathFrame = CloudDeathEffect.Frames.GetFrameCount("Dissolve") -1;
    }
    public void TakeDamage(float damage, Vector2? attackFromVector)
    {
        Health -= damage;     // Decrease the health of the spiked fly
        HealthPoints.Text = "HP:" + Health;
        // If health is below or equal to zero & it's not dead
        if(Health <= 0 && !Dead)
        {
            Dead = true;
            GetNode<AnimatedSprite>("AnimatedSprite").Visible = false;
            CloudDeathEffect.Visible = true;
            CloudDeathEffect.Play("Dissolve");
        }
        else
        {
            // If the attack from vector isn't null
            if(attackFromVector != null)
            {
                // Knockback 
                var angle = this.Position.AngleTo((Vector2)attackFromVector);       // Get the angle from the attack
                var direction = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));   // Calculate the new direction
                this.Velocity = direction * 200;                                    // Set the new direction for the sprite
            }
            this.Modulate = new Color(1,0,1,1); // Set the color of the sprite to magenta
            TakeDamageTimer.Start();            // Start the take damage timer
        }
    }
    private void OnTakeDamageTimerTimeout()
    {
        this.Modulate = new Color(1,1,1,1);     // Restore color to normal
    }    
     public override void _Process(float delta)
     {
        // If the sprite is dead, and the final frame in the death cloud animation has played
        if(Dead && CloudDeathEffect.Frame == lastCloudDeathFrame)
        {
            this.QueueFree();   // Free the sprite from memory
            return;
        }
        // If the y-velocity isn't zero
        if(Velocity.y != 0)
        {
            // Lerp the position back to zero
            Velocity.y = Mathf.Lerp(Velocity.y, 0, 0.01f);
        }
        // If the speed of the sprite isn't at it's normal value
        if(Velocity.x > -SpikedFlySpeed)
        {
            // Move the speed back towards it's normal value
            Velocity.x -= SpikedFlySpeed * delta;
        }
        // If the sprite still has health
        if(Health>0)
        {
            // Run the built-in move and slide method to move the sprite with physics
            MoveAndSlide(Velocity, Vector2.Up,false);
        }
     }
}
