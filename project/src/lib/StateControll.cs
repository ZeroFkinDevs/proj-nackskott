using Godot;
using System;

namespace Game
{
    public interface IState
    {

    }
    public interface IStateControlling<T>
    {
        public bool SetState(T state);
    }
}