using Godot;
using System;

namespace Game
{
    /// <summary>
    /// Описывает объект который может иметь идентификатор
    /// </summary>
    public interface IIdentable
    {
        public string GetID();
    }
}