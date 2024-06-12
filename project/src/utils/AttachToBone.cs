using Godot;
using System;

namespace Game
{
    public partial class AttachToBone : Node3D
    {
        [Export]
        public AnimationController AnimController;
        [Export]
        public string BoneName;

        private int boneId;

        public override void _Ready()
        {
            boneId = AnimController.skeleton3D.FindBone(BoneName);
        }

        public override void _Process(double delta)
        {
            var pose = AnimController.GetBoneGlobalPose(boneId);
            GlobalTransform = pose;
        }
    }
}
