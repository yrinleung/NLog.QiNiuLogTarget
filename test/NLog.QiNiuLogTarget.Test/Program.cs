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
                try
                {
                    throw new Exception("test");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                Console.WriteLine("success");
                Console.ReadKey();
            }
        }
    }
}
