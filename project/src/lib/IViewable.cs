using Godot;
using System;

namespace Game
{
    public interface IViewable
    {
        public Vector3 GetViewTargetPoint();
    }
    public interface IViewFollower<T>
    {
        public T ViewTarget { get; }
        public void SetViewTarget(T target);
    }
}