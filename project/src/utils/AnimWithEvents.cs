using Godot;
using System;
using Godot.Collections;
using Game.Utils;

using Dictionary = Godot.Collections.Dictionary<string, Godot.Variant>;
using ArrayOfDicts = Godot.Collections.Array<Godot.Collections.Dictionary<string, Godot.Variant>>;
using ArrayOfStrings = Godot.Collections.Array<string>;

namespace Game
{
    public partial class AnimWithEvents : AnimationPlayer
    {
        [Export]
        public Dictionary AnimationEvents;

        private delegate void AnimEventHandler(ArrayOfStrings args);
        private System.Collections.Generic.Dictionary<string, AnimEventHandler> EventsHandlersMap;
        private static System.Collections.Generic.HashSet<string> _registeredEvents;
        public AnimWithEvents()
        {
            _registeredEvents = new System.Collections.Generic.HashSet<string>();
            EventsHandlersMap = new System.Collections.Generic.Dictionary<string, AnimEventHandler> {
                { "attach_camera",  AttachCamera},
                { "deattach_camera",  DeAttachCamera}
            };
        }

        public void AttachCamera(ArrayOfStrings args){
            var cameraAnimNode = GetParent().GetNode<Node3D>("camera");
            if(cameraAnimNode==null) return;
            Global.Instance.CurrentMainCamera.StartAttachedPerspectiveSession(cameraAnimNode);
        }
        public void DeAttachCamera(ArrayOfStrings args){
            Global.Instance.CurrentMainCamera.DeAttach();
        }


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetupEvents();
        }

        public void SetupEvents()
        {
            foreach (string animKey in AnimationEvents.Keys)
            {
                Animation anim = GetAnimation(animKey);
                if (anim == null){
                    GD.Print(animKey, " not found");
                }
                if (anim != null)
                {
                    string regKey = anim.ResourcePath;
                    if (_registeredEvents.Contains(regKey)) continue;

                    int trackIdx = anim.AddTrack(Animation.TrackType.Method);
                    anim.TrackSetPath(trackIdx, new NodePath(this.Name));

                    foreach (Godot.Collections.Dictionary eventData in AnimationEvents.Get<Godot.Collections.Array>(animKey))
                    {
                        float time = (float)eventData.Get<double>("time");
                        string event_key = eventData.Get<string>("event");
                        ArrayOfStrings args = new ArrayOfStrings(eventData.Get<Godot.Collections.Array>("args"));
                        Dictionary newKey = new Dictionary{
                            {"args", new Array<Variant>(new Variant[]{ 
                                event_key, 
                                args })}, 
                            {"method", "HandleEvent"}
                        };
                        anim.TrackInsertKey(trackIdx, time, newKey);
                        int keyIdx = anim.TrackFindKey(trackIdx, time, Animation.FindMode.Approx);
                    }

                    _registeredEvents.Add(regKey);
                }
            }
        }

        public void HandleEvent(string event_key, ArrayOfStrings args)
        {
            if (EventsHandlersMap.ContainsKey(event_key))
            {
                try
                {
                    EventsHandlersMap[event_key](args);
                }
                catch(Exception e)
                {
                    GD.PrintErr(e);
                }
            }
        }
    }
}