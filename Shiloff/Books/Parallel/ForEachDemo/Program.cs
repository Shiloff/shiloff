using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ForEachDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = Console.ReadLine();
            var zise = DirectoryBytes(path, "*.exe", SearchOption.AllDirectories,
                (name, size) => Console.WriteLine($"{name} size: {size} bytes"));

            Console.WriteLine(zise);

            Console.ReadKey();
        }

        private static long DirectoryBytes(string path, string searchPattern, SearchOption searchOption, Action<string, long> log )
        {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            long masterTotal = 0;
            var result = Parallel.ForEach<string, long>(
                files,
                () =>
                {
                    // localInit: вызывается в момент запуска задания
                    // Инициализация: задача обработала 0 байтов
                    return 0; // Присваивает taskLocalTotal начальное значение 0
                },
                (file, loopState, index, taskLocalTotal) =>
                {
                    // body: Вызывается
                    // один раз для каждого элемента
                    // Получает размер файла и добавляет его к общему размеру
                    long fileLength = 0;
                    FileStream fs = null;
                    try
                    {
                        fs = File.OpenRead(file);
                        fileLength = fs.Length;
                    }
                    catch (IOException)
                    {
                        /* Игнорируем файлы, к которым нет доступа */
                    }
                    finally
                    {
                        if (fs != null) fs.Dispose();
                    }
                    log(Path.GetFileName(file), fileLength);
                    return taskLocalTotal + fileLength;
                },
                taskLocalTotal =>
                {
                    // localFinally: Вызывается один раз в конце задания
                    // Атомарное прибавление размера из задания к общему размеру
                    Interlocked.Add(ref masterTotal, taskLocalTotal);
                });
            return masterTotal;
        }
    }
}