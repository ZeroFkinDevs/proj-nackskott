using Godot;
using Godot.Collections;
using System;

namespace Game {
	/// <summary>
	/// Класс описывающий в себе настройки игры.
	/// Различные параметры. которые можно будет сериализовать в json.
	/// </summary>
	public class GameSettings : ISerializable
	{
		public bool IsInDebug = false;

        /// <summary>
        /// Делегат функции события OnSettingsChanged в настройках, которая вызывается, когда меняются настройки.
        /// </summary>
        /// <param name="settings"></param>
        public delegate void OnSettingsChangedFunc(GameSettings settings);
        /// <summary>
        /// Событие OnSettingsChanged. вызывается, когда меняются настройки
        /// </summary>
        public event OnSettingsChangedFunc OnSettingsChanged;

        /// <summary>
        /// Вызывает все функции, которые подписаны на событие OnSettingsChanged
        /// </summary>
        public void InvokeChange()
        {
            // вызываем все функции которые подписаны на событие. используется делегат OnSettingsChangedFunc
            OnSettingsChanged?.Invoke(this);
        }

        public ISerializable DeserializeFromDict(Dictionary dict)
        {
            IsInDebug = (bool)dict["IsInDebug"];
            OnSettingsChanged?.Invoke(this);
            return this;
        }

        public Dictionary SerializeToDict()
        {
            return new Godot.Collections.Dictionary
            {
                {"IsInDebug", IsInDebug},
            };
        }
    }
}
