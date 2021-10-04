using System;
using System.Threading.Tasks;

namespace TaskCreateStart
{
    public static class Main1
    {
        public static void Execute()
        {
            // Способ 1. Создает и запускает task.
            Task.Factory.StartNew(() => Write('.'));

            // Способ 2. Отдельно создает и запускает task.
            var t = new Task(() => Write('?'));
            t.Start();

            Write('-');

            Console.WriteLine("Main program done.");
            Console.ReadKey();
        }

        public static void Write(char c)
        {
            var i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }
    }
}
