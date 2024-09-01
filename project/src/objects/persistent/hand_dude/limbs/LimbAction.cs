using Godot;
using System;

namespace Game
{
    
    public partial class LimbAction : Node3D
    {
        [Export]
        public Node3D Target;
        [Export]
        public AnimationPlayer AnimController;

        public event Action OnFinished;

        public Node3D Act(){
            AnimController.AnimationFinished += (name) => {
                OnFinished?.Invoke();
                Target.QueueFree();
                QueueFree();
            };
            return Target;
        }
    }
}
