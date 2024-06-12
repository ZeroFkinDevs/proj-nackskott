using Game.Utils;
using Godot;
using Godot.Collections;

namespace Game
{
    public partial class Slicer : CharacterBody3D
    {
        [Export]
        public Area3D area;
        public bool Attacked = false;
        public Node3D target;

        [Export]
        public AnimationPlayer AnimPlayer;
        [Export]
        public AudioStreamPlayer3D AudPlayer;
        [Export]
        public AudioStream AttackSound;

        public float TurnOffTimer = 0.0f;
        public bool TurningOff = false;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            foreach(var body in area.GetOverlappingBodies()){
                if(!Attacked){
                    if(body is NPCController){
                        target = body;
                        Attack();
                    }
                    if(body is HandCharacter){
                        target = body;
                        Attack();
                    }
                }
            }


            if(TurningOff){
                TurnOffTimer += (float)delta;
                AnimPlayer.SpeedScale = 1.0f / (1.0f + TurnOffTimer / 1.0f);
            }
        }

        public async void Attack(){
            TurningOff = true;
            Attacked = true;
            AudPlayer.Stream = AttackSound;
            AudPlayer.Play(0.0f);

            var p1 = GlobalPosition;
            var dir = GlobalTransform.Basis.Z; 
            var p2 = GlobalPosition + dir * 100.0f;
            var space = GetWorld3D().DirectSpaceState;
            var prms = PhysicsRayQueryParameters3D.Create(p1, p2, 2);
            var result = space.IntersectRay(prms);
            // проверить есть ли препятствия
            
            if(result.Count == 0)
                return;
        
            var pos = result.Get<Vector3>("position");
            pos -= dir * 0.4f;

            // GlobalPosition = pos;
            Tween tween = GetTree().CreateTween();
            var time = 0.6f;
            tween.TweenProperty(this, "global_position", pos, time).SetTrans(Tween.TransitionType.Elastic);
            await ToSignal(GetTree().CreateTimer(time*0.5f), "timeout");
            if(target!=null){
                target.GlobalPosition = pos;
                if(target is IKillable killable){
                    killable.Kill();
                }
            }
        }
    }
}