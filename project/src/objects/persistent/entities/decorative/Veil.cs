using Godot;
using System;

namespace Game
{
    public partial class Veil : Area3D
    {
        [Export] 
        Node3D Model;
        float RotationVelocity = 1.0f;

        public override void _Ready()
        {
            base._Ready();
            BodyEntered += OnBodyEntered;
        }
        public void OnBodyEntered(Node3D body){
            if(body is CharacterBody3D character){
                RotationVelocity -= Model.GlobalBasis.Z.Normalized().Dot(character.Velocity.Normalized()) * 100.0f * character.Velocity.Length();
            }
        }
        public override void _Process(double delta)
        {
            var degrees = Model.RotationDegrees / (1.0f+(float)delta);
            RotationVelocity -= degrees.X / 2.0f;
            degrees.X += RotationVelocity * (float)delta;
            Model.RotationDegrees = degrees;
        }
    }
}