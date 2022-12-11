using Godot;
using System;

public class Main : Node2D
{

    private PackedScene SpikedFly;
    private Timer SpawnTimer;
    private RandomNumberGenerator Rnd = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Hidden);
        Rnd.Randomize();
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        SpikedFly = ResourceLoader.Load<PackedScene>("res://Scenes/Enemies/SpikedFly/SpikedFly.tscn");
        SpawnSpikedFly();
    }
    private void SpawnSpikedFly()
    {
        var ypos = Rnd.RandfRange(100,1000);
        var newSpikedFly = SpikedFly.Instance() as SpikedFly;
        newSpikedFly.Position = new Vector2(2000,ypos);
        this.AddChild(newSpikedFly);
    }
    private void OnSpawnTimerTimeout()
    {
        SpawnSpikedFly();
        SpawnTimer.WaitTime = Rnd.RandfRange(1,4);
    }
}
