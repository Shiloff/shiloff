using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace ISerializableDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                var myClass = new MyClass(10);
                formatter.Serialize(stream, myClass);

                stream.Position = 0;
                myClass = null;
                myClass = (MyClass) formatter.Deserialize(stream);

                Console.WriteLine(myClass.X);
                Console.WriteLine(myClass.InnerClass.Y);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, new CustomException());
                stream.Position = 0;
                var exception = (CustomException)formatter.Deserialize(stream);

                Console.WriteLine(exception.ToString());
            }
            Console.ReadKey();
        }
    }

    [Serializable]
    public sealed class InnerClass
    {
        public int Y { get; set; }

    }

    [Serializable]
    public sealed class MyClass : ISerializable, IDeserializationCallback
    {

        public readonly int X;
        public InnerClass InnerClass;
        public MyClass(int x)
        {
            X = x;
            InnerClass = new InnerClass() {Y = 100};
        }

        private MyClass(SerializationInfo info, StreamingContext context)
        {
            X = info.GetInt32("X");
            InnerClass = (InnerClass)info.GetValue("InnerClass", typeof(InnerClass));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", X);
            info.AddValue("InnerClass", InnerClass);
        }

        public void OnDeserialization(object sender)
        {
            //throw new NotImplementedException();
        }
    }

    [Serializable]
    public sealed class CustomException : Exception
    {
        private string _add;

        public CustomException() : base()
        {
            _add = "Add";
        }

        public CustomException(string message) : base(message)
        {

        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {

        }

        private CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _add = info.GetString("add");
        }

        public override string ToString()
        {

            return $"{_add}\r\n{base.ToString()}";
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("add", _add);

            base.GetObjectData(info, context);
        }
    }
}