using System;
using Godot;

namespace Game
{
    /// <summary>
    /// Грубо говоря, ноги NPC. Просто интерфейс для передвижения
    /// Здесь не должно быть ссылки на NPC BRAIN!!!
    /// </summary>
    public partial class NPCController : CharacterBody3D, IDamagable, IKillable
    {
        private float gravity = 10.0f;
        
        [Export]
        public float Speed;
        [Export]
        public float MaxHealth = 10;
        public float Health;
        public int Inconfidence = 0;
        [Export]
        public float RotationSpeed = 1.0f;
        [Export]
        public float RigidPushForce = 1.0f;
        [Export]
        public NPCAnimationController AnimController;
        [Export]
        public DamageArea damageArea;
        [Export]
        public AudioStreamPlayer3D StepsSoundPlayer;
        [Export]
        public NodePath NavAgentPath;
        public NavigationAgent3D NavAgent;

        public Vector3 RotationDirection;

        [Export]
        public Godot.Collections.Array<PackedScene> ItemsToDrop;

        [Export]
        public Node3D HpLabel;

        [Export]
        public SoundBank DeathSounds;

        Vector3 safeVelocity;

        private bool _alive = true;
        public bool Alive {
            get{
                return _alive;
            }
            set{
                _alive = value;
                OnLivingStateChange?.Invoke(value);
            }
        }
        public event Action<bool> OnLivingStateChange;

        #region Closed
        public override void _Ready()
        {
            base._Ready();
            Health = MaxHealth;
            NavAgent = GetNode<NavigationAgent3D>(NavAgentPath);
            NavAgent.VelocityComputed += OnVelocityComputed;

            if(damageArea!=null && AnimController.attackMesh!=null){
                damageArea.Reparent(AnimController.attackMesh);
                damageArea.Position = Vector3.Zero;
            }
        }
        public override void _Process(double delta)
        {
            base._Process(delta);

            HpLabel.Set("text", "hp: "+Health.ToString()+"\n"+"inconf: "+Inconfidence.ToString());
        }
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            ProcessGoingToPoint(delta);
            if(Alive) ProcessMovement(delta);
            ProcessRigidPush(delta);
            ProcessRotation(delta);
            ProcessSounds();
        }
        private void ProcessGoingToPoint(double delta){
            if(NavAgent.IsNavigationFinished()){
                AnimController.Idle();
                return;
            }
            
            var currentPos = GlobalPosition;
            var nextPos = NavAgent.GetNextPathPosition();
            NavAgent.Velocity = AnimController.GetMovementVector();

            var viewPoint = new Vector3(nextPos.X, GlobalPosition.Y, nextPos.Z);
            Vector3 direction = currentPos.DirectionTo(viewPoint);
            direction.Y = 0;
            RotationDirection = direction;
            AnimController.Walk(Inconfidence);

            if(safeVelocity.Length()>0.1f){

            }else{
                // AnimController.Idle();
            }
        }
        private void ProcessMovement(double delta)
        {
            Vector3 moveVec;
            if(NavAgent.IsNavigationFinished()){
                moveVec = AnimController.GetMovementVector();
            }else{
                moveVec = safeVelocity;
            }
            Velocity = new Vector3(moveVec.X, Velocity.Y, moveVec.Z);
            Velocity -= Vector3.Up * gravity * (float)delta;
            var velocityCopy = Velocity;
            MoveAndSlide();
            var colInfo = GetLastSlideCollision();
        }
        private void ProcessRigidPush(double delta){
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var colInfo = GetSlideCollision(i);
                var collider = colInfo.GetCollider();
                if(collider is RigidBody3D body){
                    body.ApplyCentralImpulse(-colInfo.GetNormal() * RigidPushForce);
                }

            }
        }
        public void ProcessRotation(double delta){
            if(!RotationDirection.IsZeroApprox()){
                var trans = GlobalTransform;
                var rotSpeed = RotationSpeed;
                if(AnimController.IsAttacking()) rotSpeed *= 2.0f;
                trans.Basis = new Basis(
                    trans.Basis.GetRotationQuaternion().Slerp(
                        Basis.LookingAt(RotationDirection, Vector3.Up).GetRotationQuaternion(), 
                        rotSpeed*(float)delta));
                GlobalTransform = trans;
            }
        }
        public void ProcessSounds(){
            if(StepsSoundPlayer!=null){
                StepsSoundPlayer.VolumeDb = Mathf.Min(new Vector2(Velocity.X, Velocity.Z).Length()*50-50, 0);
            }
        }
        #endregion

        public void OnVelocityComputed(Vector3 _safeVelocity){
            safeVelocity = _safeVelocity;
        }

        public void RotateToPoint(Vector3 point)
        {
            RotationDirection = GlobalPosition.DirectionTo(point)*new Vector3(1,0,1);
        }
        public void GoToPoint(Vector3 point)
        {
            NavAgent.TargetPosition = point;
        }
        public void StopMoving(){
            NavAgent.TargetPosition = GlobalPosition;
        }

        public void TakeDamage(float dmg, Vector3 position, Node from=null){
            AnimController.Stun();
            Health -= dmg;
            if(from is Node3D n3d){
                Global.Instance.resources.EmitBlood(this, n3d.GlobalPosition);
            }else{
                Global.Instance.resources.EmitBlood(this);
            }
        }

        public void Attack(string attackName = "attack"){
            if(AnimController.IsAttacking()) return;
            AnimController.Attack(attackName);
        }

        public void Kill(Node from=null){
            DeathSounds?.PlayAtNode(this);
            Alive = false;
            Health = 0.0f;
            Node3D blood = null;
            if(from is Node3D n3d){
                blood = Global.Instance.resources.EmitBlood(this, n3d.GlobalPosition);
            }else{
                blood = Global.Instance.resources.EmitBlood(this);
            }
            blood.Scale = Vector3.One * 2.0f;

            foreach (var item in ItemsToDrop)
            {
                var obj = item.Instantiate();
                this.AddChild(obj);
                obj.Reparent(Global.Instance.GameScene);
                if(obj is Node3D obj3d){
                    obj3d.Transform = obj3d.Transform.Translated(Vector3.Up*5.0f);
                }
                if(obj is RigidBody3D body){
                    if(from is Node3D from3d){
                        body.ApplyCentralForce(body.GlobalPosition.DirectionTo(from3d.GlobalPosition)*5000.0f);
                    }
                }
            }
        }
    }
}
