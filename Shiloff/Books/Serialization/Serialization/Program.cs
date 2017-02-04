using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace Serialization
{
    internal class Program
    {
        public static void Main()
        {
            // Создание графа объектов для последующей сериализации в поток
            var objectGraph = new string[]
            {
                "Jeff",
                "Kristin",
                "Aidan",
                "Grant"
            };
            var stream = SerializeToMemory(objectGraph);
            // Обнуляем все для данного примера
            stream.Position = 0;
            objectGraph = null;
            // Десериализация объектов и проверка их работоспособности
            objectGraph = (string[]) DeserializeFromMemory(stream);
            foreach (var s in objectGraph)
                Console.WriteLine(s);
        }

        private static MemoryStream SerializeToMemory(object objectGraph)
        {
            // Конструирование потока, который будет содержать
            var stream = new MemoryStream();
            // Задание форматирования при сериализации
            var formatter = new BinaryFormatter();
            // Заставляем модуль форматирования сериализовать объекты в поток
            formatter.Serialize(stream, objectGraph);
            // Возвращение потока сериализованных объектов вызывающему методу
            return stream;
        }
        private static object DeserializeFromMemory(Stream stream)
        {
            // Задание форматирования при сериализации
            var formatter = new BinaryFormatter();
            // Заставляем модуль форматирования десериализовать объекты из потока
            return formatter.Deserialize(stream);
        }

    }
}
    
