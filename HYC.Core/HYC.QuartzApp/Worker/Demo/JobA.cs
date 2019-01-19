using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HYC.QuartzApp.Worker.Demo
{
    /// <summary>
    /// JobA
    /// DisallowConcurrentExecution表示(不能重复执行)
    /// </summary>
    [DisallowConcurrentExecution]
    public class JobA : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync($"A 开始 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            await Task.Delay(20000);

            await Console.Out.WriteLineAsync($"A 结束 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
}
