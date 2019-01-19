using System;
namespace HYC.QuartzApp
{
    class Program
    {
       
        static void Main(string[] args)
        {
            //初始化Hander
            InitSchedule.InitHander().GetAwaiter().GetResult();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }
    }
}
