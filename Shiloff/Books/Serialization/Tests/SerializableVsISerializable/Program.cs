using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace SerializableVsISerializable
{
    class Program
    {
        static void Main(string[] args)
        {

            var class1 = new Class1(56);
            var class2 = new Class2(56);

            using (var watcher = new Watcher("[Serializable]", WatcherType.Time | WatcherType.Tics))
            {
                var fromatter = new BinaryFormatter();
                for (var i = 0; i < 100000; i++)
                {
                    using (var stream = new MemoryStream())
                    {
                        fromatter.Serialize(stream, class1);
                        class1 = null;
                        stream.Position = 0;
                        class1 = (Class1) fromatter.Deserialize(stream);
                    }
                }
            }

          
            using (var watcher = new Watcher("ISerializable", WatcherType.Time | WatcherType.Tics))
            {
                var fromatter = new BinaryFormatter();
                for (var i = 0; i < 100000; i++)
                {
                    using (var stream = new MemoryStream())
                    {
                        fromatter.Serialize(stream, class2);
                        class2 = null;
                        stream.Position = 0;
                        class2 = (Class2) fromatter.Deserialize(stream);
                    }
                }
            }
            Console.ReadKey();
        }
    }

    [Serializable]
    public class Class1
    {
        public int A1;
        public int A2;
        public int A3;
        public int A4;
        public int A5;
        public int A6;
        public int A7;
        public int A8;
        public int A9;
        public Class1(int a0)
        {
            A1 = a0*1;
            A2 = a0*2;
            A3 = a0*3;
            A4 = a0*4;
            A5 = a0*5;
            A6 = a0*6;
            A7 = a0*7;
            A8 = a0*8;
            A9 = a0*9;
        }

    }

    [Serializable]
    public class Class2 : ISerializable
    {

        public int A1;
        public int A2;
        public int A3;
        public int A4;
        public int A5;
        public int A6;
        public int A7;
        public int A8;
        public int A9;
        public Class2(int a0)
        {
            A1 = a0 * 1;
            A2 = a0 * 2;
            A3 = a0 * 3;
            A4 = a0 * 4;
            A5 = a0 * 5;
            A6 = a0 * 6;
            A7 = a0 * 7;
            A8 = a0 * 8;
            A9 = a0 * 9;
        }

        private Class2(SerializationInfo info, StreamingContext context)
        {
            A1 = info.GetInt32("A1");
            A2 = info.GetInt32("A2");
            A3 = info.GetInt32("A3");
            A4 = info.GetInt32("A4");
            A5 = info.GetInt32("A5");
            A6 = info.GetInt32("A6");
            A7 = info.GetInt32("A7");
            A8 = info.GetInt32("A8");
            A9 = info.GetInt32("A9");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("A1", A1);
            info.AddValue("A2", A2);
            info.AddValue("A3", A3);
            info.AddValue("A4", A4);
            info.AddValue("A5", A5);
            info.AddValue("A6", A6);
            info.AddValue("A7", A7);
            info.AddValue("A8", A8);
            info.AddValue("A9", A9);
        }
    }
}
