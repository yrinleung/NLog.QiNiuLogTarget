using System;

namespace NLog.QiNiuLogTarget.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = LogManager.GetCurrentClassLogger();
            while (true)
            {
                log.Error("high Hkaos four");
                Console.WriteLine("success");
                Console.ReadKey();
            }
        }
    }
}
