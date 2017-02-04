using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading;
using Helpers;

namespace BinaryVsSoapFormatters
{

    public interface IFormatterFactory
    {
        IFormatter Create();
    }

    public class BinaryFormatterFactory : IFormatterFactory
    {
        public IFormatter Create()
        {
            return new BinaryFormatter();
        }
    }

    public class SoapFormatterFactory : IFormatterFactory
    {
        public IFormatter Create()
        {
            return new SoapFormatter();
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            var objectGraph = new string[]
            {
                "Jeff",
                "Kristin",
                "Aidan",
                "Grant"
            };

            using (var watcher = new Watcher("BinaryFormatter", WatcherType.Time | WatcherType.Tics))
            {
                var binariSerializer = new Serializer(new BinaryFormatterFactory());
                for (var i = 0; i < 100000; i++)
                {
                    var stream = binariSerializer.SerializeToMemory(objectGraph);
                    stream.Position = 0;
                    var obj = binariSerializer.DeserializeFromMemory(stream);
                }

            }

            using (var watcher = new Watcher("SoapFormatter", WatcherType.Time | WatcherType.Tics))
            {
                var binariSerializer = new Serializer(new SoapFormatterFactory());
                for (var i = 0; i < 100000; i++)
                {
                    var stream = binariSerializer.SerializeToMemory(objectGraph);
                    stream.Position = 0;
                    var obj = binariSerializer.DeserializeFromMemory(stream);
                }
            }
            Console.ReadKey();
        }
    }

    public class Serializer
    {
        private readonly IFormatterFactory _formatterFactory;

        public Serializer(IFormatterFactory formatterFactory)
        {
            _formatterFactory = formatterFactory;
        }
        public MemoryStream SerializeToMemory(object objectGraph)
        {
            // Конструирование потока, который будет содержать
            var stream = new MemoryStream();
            // Задание форматирования при сериализации
            var formatter = _formatterFactory.Create();
            // Заставляем модуль форматирования сериализовать объекты в поток
            formatter.Serialize(stream, objectGraph);
            // Возвращение потока сериализованных объектов вызывающему методу
            return stream;
        }

        public object DeserializeFromMemory(Stream stream)
        {
            // Задание форматирования при сериализации
            var formatter = _formatterFactory.Create();
            // Заставляем модуль форматирования десериализовать объекты из потока
            return formatter.Deserialize(stream);
        }

    }
}