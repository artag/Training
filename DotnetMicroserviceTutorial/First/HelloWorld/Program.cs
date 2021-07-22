using System;
using Serilog;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var data = new LogData()
            {
                Message = "Hello World",
                Number = 123
            };

            Log.Information("Simple message");

            var number = 123;
            Log.Warning("This is number {number}", number);

            Log.Error(new ArgumentOutOfRangeException(), "Hello world!");

            Log.Information("{Message}, {Number}", data.Message, data.Number);
            Log.Information("{@log}", data);
        }
    }

    class LogData
    {
        public string Message { get; set; }
        public int Number { get; set; }
    }
}
