using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Грубо говоря, ноги NPC. Просто интерфейс для передвижения
    /// </summary>
    public partial class NPCController : CharacterBody3D, IDamagable
    {
        [Export]
        public float Speed;
        [Export]
        public float MaxHealth = 10;
        public float Health;
        public int Inconfidence = 0;
        [Export]
        public float RotationSpeed = 1.0f;
        [Export]
        public float AttackTimerWait = 2.0f;
        float attackTimer = 0.0f;
        [Export]
        public NPCAnimationController AnimController;
        [Export]
        public DamageArea damageArea;
        [Export]
        public NodePath NavAgentPath;
        public NavigationAgent3D NavAgent;

        [Export]
        public Node3D HpLabel;

        Vector3 safeVelocity;

        #region Closed
        public override void _Ready()
        {
            base._Ready();
            Health = MaxHealth;
            NavAgent = GetNode<NavigationAgent3D>(NavAgentPath);
            NavAgent.VelocityComputed += OnVelocityComputed;

            damageArea.Reparent(AnimController.attackMesh);
            damageArea.Position = Vector3.Zero;
        }
        public override void _Process(double delta)
        {
            base._Process(delta);

            HpLabel.Set("text", "hp: "+Health.ToString()+"\n"+"inconf: "+Inconfidence.ToString());
            
            if(attackTimer>0.0f) attackTimer -= (float)delta;
        }
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            ProcessMovement(delta);
        }
        private void ProcessMovement(double delta)
        {
            if(NavAgent.IsNavigationFinished()){
                AnimController.Idle();
                return;
            }
            var currentPos = GlobalPosition;
            var nextPos = NavAgent.GetNextPathPosition();
            // NavAgent.Velocity = currentPos.DirectionTo(nextPos) * Speed;
            NavAgent.Velocity = AnimController.GetMovementVector().Slerp(currentPos.DirectionTo(nextPos) * Speed, 0.1f);
            
            var viewPoint = new Vector3(nextPos.X, GlobalPosition.Y, nextPos.Z);
            Velocity = safeVelocity;
            // Velocity = safeVelocity;
            // if(AnimController.IsStunned()) Velocity *= new Vector3(-1, 1, -1);

            var trans = GlobalTransform;
            Vector3 direction = currentPos.DirectionTo(viewPoint);
            if(!direction.IsZeroApprox()){
                var rotSpeed = RotationSpeed;
                if(AnimController.IsAttacking()) rotSpeed *= 2.0f;
                trans.Basis = new Basis(
                    trans.Basis.GetRotationQuaternion().Slerp(
                        Basis.LookingAt(direction, Vector3.Up).GetRotationQuaternion(), 
                        rotSpeed*(float)delta));
                GlobalTransform = trans;
            }
            AnimController.Walk(Inconfidence);
            if(safeVelocity.Length()>0.1f){

            }else{
                // AnimController.Idle();
            }
            MoveAndSlide();
        }
        #endregion

        public void OnVelocityComputed(Vector3 _safeVelocity){
            safeVelocity = _safeVelocity;
        }

        public void GoToPoint(Vector3 point)
        {
            NavAgent.TargetPosition = point;
        }

        public void TakeDamage(float dmg, Vector3 position, Node from=null){
            AnimController.Stun();
            Health -= dmg;
        }

        public void Attack(float dmg=0.0f){
            if(AnimController.IsAttacking()) return;
            if(attackTimer>0.0f) return;
            AnimController.Attack();
            attackTimer = AttackTimerWait;
        }
    }
}
