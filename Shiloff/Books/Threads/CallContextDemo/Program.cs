using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers;

namespace CallContextDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CallContext.LogicalSetData("Name", "Jeffrey");
            // Заставляем поток из пула работать
            // Поток из пула имеет доступ к данным контекста логического вызова
            ThreadPool.QueueUserWorkItem(
                        state => CallContext.LogicalGetData("Name"));
            using (var watcher = new Watcher("Flow", WatcherType.Tics))
            {
                for (var i = 0; i < 100; i++)
                {
                    ThreadPool.QueueUserWorkItem(state => CallContext.LogicalGetData("Name"));
                }
            }

            // Запрещаем копирование контекста исполнения потока метода Main
            ExecutionContext.SuppressFlow();
            // Заставляем поток из пула выполнить работу.
            // Поток из пула НЕ имеет доступа к данным контекста логического вызова

            using (var watcher = new Watcher("SuppressFlow", WatcherType.Tics))
            {
                for (var i = 0; i < 100; i++)
                {
                    ThreadPool.QueueUserWorkItem(state => CallContext.LogicalGetData("Name"));
                }
            }
            // Восстанавливаем копирование контекста исполнения потока метода Main
            // на случай будущей работы с другими потоками из пула
            ExecutionContext.RestoreFlow();

            WaitCallback action = state => CallContext.LogicalGetData("Name");
            Action action2 = () => CallContext.LogicalGetData("Name");

            Action action3 = () => Console.WriteLine(CallContext.LogicalGetData("Name"));
            while (Console.ReadKey().Key != ConsoleKey.Backspace)
            {
                ExecutionContext.SuppressFlow();
                const int counts = 1000;
                using (var watcher = new Watcher("SuppressFlow", WatcherType.Tics))
                {
                    for (var i = 0; i < counts; i++)
                    {
                        ThreadPool.QueueUserWorkItem(action);
                    }
                }

                using (var watcher = new Watcher("Task.Run", WatcherType.Tics))
                {
                    for (var i = 0; i < counts; i++)
                    {
                        Task.Run(action2);
                    }
                }

                using (var watcher = new Watcher("Task.Run", WatcherType.Tics))
                {
                    for (var i = 0; i < 1; i++)
                    {
                        Task.Run(action3);
                    }
                }

                ExecutionContext.RestoreFlow();
                using (var watcher = new Watcher("RestoreFlow", WatcherType.Tics))
                {
                    for (var i = 0; i < counts; i++)
                    {
                        ThreadPool.QueueUserWorkItem(action);
                    }
                }

                using (var watcher = new Watcher("Task.Run", WatcherType.Tics))
                {
                    for (var i = 0; i < counts; i++)
                    {
                        Task.Run(action2);
                    }
                }
                using (var watcher = new Watcher("Task.Run", WatcherType.Tics))
                {
                    for (var i = 0; i < 1; i++)
                    {
                        Task.Run(action3);
                    }
                }
            }
            Console.ReadLine();
        }

        private static void CallContextTest(object state)
        {
            CallContext.LogicalGetData("Name");
        }
    }
}
