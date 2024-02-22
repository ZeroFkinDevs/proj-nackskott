using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Грубо говоря, ноги NPC. Просто интерфейс для передвижения
    /// </summary>
    public partial class NPCController : CharacterBody3D
    {
        [Export]
        public float Speed;
        [Export]
        public NodePath NavAgentPath;
        public NavigationAgent3D NavAgent;

        Vector3 safeVelocity;

        #region Closed
        public override void _Ready()
        {
            base._Ready();
            NavAgent = GetNode<NavigationAgent3D>(NavAgentPath);
            NavAgent.VelocityComputed += OnVelocityComputed;
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
        }
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            ProcessMovement(delta);
        }
        private void ProcessMovement(double delta)
        {
            if(NavAgent.IsNavigationFinished()){
                return;
            }
            var currentPos = GlobalPosition;
            var nextPos = NavAgent.GetNextPathPosition();
            NavAgent.Velocity = currentPos.DirectionTo(nextPos) * Speed;
            Velocity = safeVelocity;
            MoveAndSlide();
        }
        #endregion

        public void OnVelocityComputed(Vector3 _safeVelocity){
            safeVelocity = _safeVelocity;
            GD.Print(safeVelocity);
        }


        public void GoToPoint(Vector3 point)
        {
            NavAgent.TargetPosition = point;
        }
    }
}
