using Godot;
using System;
using System.Reflection;

namespace Game
{
    /// <summary>
    /// Описывает объект который можно сериализовать и десериализовать.
    /// </summary>
    public interface ISerializable
    {
        // TODO: реализовать без этих функций в каждом классе
        // Либо самому на System.Reflection делать, либо Newtonsoft.Json использовать, как раньше

        /// <summary>
        /// Извлекает данные и создает словарь с ними.
        /// </summary>
        /// <returns>Godot словарь с данными</returns>
        public Godot.Collections.Dictionary SerializeToDict();
        /// <summary>
        /// Загружает данные в созданный экземпляр.
        /// </summary>
        /// <param name="dict">данные в виде Godot словаря</param>
        /// <returns>self</returns>
        public ISerializable DeserializeFromDict(Godot.Collections.Dictionary dict);
    }
}