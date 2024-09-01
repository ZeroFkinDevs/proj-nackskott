using Godot;
using System;
using System.Diagnostics.Tracing;

namespace Game
{
    public partial class Pipe : Node3D, IIdentable
    {
        [Export]
        public string TeleportID;
        [Export]
        public string ToLocation;
        [Export]
        public Teleporter Teleport;
        [Export]
        public Gripable GripArea;
        [Export]
        public AnimationController AnimController;

        [Export]
        public SoundBank LoadSounds;
        [Export]
        public SoundBank UnloadSounds;

        protected int doorBoneId;
        protected HandDude player;

        public override void _Ready()
        {
            base._Ready();
            doorBoneId = AnimController.skeleton3D.FindBone("door");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            GripArea.GripPoint.GlobalTransform = AnimController.GetBoneGlobalPose(doorBoneId);
        }
        
        public void OnTeleportEnd(Node node){
            if(node is HandDude _player){
                player = _player;
                AnimController.SetState("state", "unload");
                player.Grip(GripArea);
                UnloadSounds.PlayAtNode(GripArea.GripPoint);
            }
        }

        public void OnHandleGrip(Node3D handle){
            AnimController.SetState("state", "load");
            Teleport.PrepareTeleportation(ToLocation);
            LoadSounds.PlayAtNode(GripArea.GripPoint);
        }

        public virtual void OnAnimationFinished(StringName animationName){
		    if(animationName == "load"){
                Teleport.Teleport(ToLocation);
            }
            if(animationName == "unload"){
                if(player != null) player.UnGripObject();
            }
		}

        public string GetID()
        {
            return TeleportID;
        }
    }
}