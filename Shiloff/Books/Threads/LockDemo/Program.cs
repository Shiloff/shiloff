using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LockDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var x = new Test(1);
            new AsyncClass().Lock(x);

            var urls = new List<string>()
            {
                "1",
                "2"
            };

            var t = urls.Select(url =>
            {
                return Task.Run(() => Console.WriteLine(url));
            });

            Task.WaitAll(t.ToArray());


            Console.ReadKey();
        }


        public class AsyncClass
        {

            public void Lock(Test arg1)
            {
                int result = DoSomeWorkAsync().Result; // 1
            }

            private async Task<int> DoSomeWorkAsync()
            {
                await Task.Delay(100).ConfigureAwait(true); //2
                return 1;
            }
        }
    }

    public class Test
    {
        public int V1 { get; }

        public Test(int v1)
        {
            V1 = v1;
        }
    }
}