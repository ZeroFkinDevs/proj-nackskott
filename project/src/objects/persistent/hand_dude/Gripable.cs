using Godot;

namespace Game
{
    public partial class Gripable : Area3D, IGripable, IUsable
    {
        [Export]
        public Node3D GripPoint;

        [Export]
        public bool IsUnGripable = true;

        [Signal]
		public delegate void OnGripEventHandler(Node3D point);

        public void Grip(Node3D user)
        {
            if(user is HandDude hand){
                hand.Grip(this);
                EmitSignal(SignalName.OnGrip, GripPoint);
            }
        }

        public Node3D GetGripPoint(){
            return GripPoint;
        }

        public bool IsEnabled()
        {
            return true;
        }
        public bool CanUnGrip(){
            return IsUnGripable;
        }

        public void Use(Node3D user){
			
		}
    }
}