using System;
using System.Diagnostics;
using System.Text;

namespace Helpers
{
    public class Watcher : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch;
        private readonly WatcherType _watcherType;

        public Watcher(string name, WatcherType type = WatcherType.Time)
        {
            _stopwatch = new Stopwatch();
            _name = name;
            _watcherType = type;
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            var str = new StringBuilder();
            str.AppendLine($"Process: {_name}");
            if ((_watcherType & WatcherType.Time) != 0)
            {
                str.AppendLine($"Elapsed time: {_stopwatch.ElapsedMilliseconds} ms.");
            }
            if ((_watcherType & WatcherType.Tics) != 0)
            {
                str.AppendLine($"Elapsed time: {_stopwatch.ElapsedTicks} tics.");
            }

            Console.Write(str.ToString());
            _stopwatch.Reset();
        }

    }

    [Flags]
    public enum WatcherType
    {
        Time = 0x01,
        Tics = 0x02,
    }
}