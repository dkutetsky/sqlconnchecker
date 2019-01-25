using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SqlConnChecker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} Checker started");
            
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var checkInterval = conf.GetValue<int?>("CheckIntervalMs");
            var connectionString = conf.GetValue<string>("ConnectionString");
            var verboseOutput = conf.GetValue<bool>("VerboseOutput");
            var writeToFile = conf.GetValue<bool>("WriteToFile");
            var fileOutputPath = conf.GetValue<string>("FileOutputPath");

            if (checkInterval == null)
            {
                Console.WriteLine($"{nameof(checkInterval)} could not be empty");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine($"{nameof(connectionString)} could not be empty");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            if (writeToFile && string.IsNullOrEmpty(fileOutputPath))
            {
                Console.WriteLine($"{nameof(fileOutputPath)} could not be empty if {nameof(writeToFile)} is true ");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            while (true)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1";
                            var commandResult = (int)command.ExecuteScalar();

                            var result = commandResult == 1;
                          
                            if (result)
                            {
                                await WriteToOutput($"{DateTime.Now} Connection is OK.", writeToFile, fileOutputPath);
                            }
                            else
                            {
                                var message = $"{DateTime.Now} Connection is NOT OK - can not execute {command.CommandText}";
                                await WriteToOutput(message, writeToFile, fileOutputPath);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var message = $"{DateTime.Now} Connection is NOT OK";
                  
                    //append exception message if verbose output is enabled
                    if (verboseOutput)
                    {
                        message += $": {e.Message}";
                    }

                    await WriteToOutput(message, writeToFile, fileOutputPath);
                }
             
                //wait some time
                await Task.Delay(checkInterval.Value);
            }
        }

        public static async Task WriteToOutput(string message, 
            bool writeToFile, 
            string fileOutputPath)
        {
            Console.WriteLine(message);

            if (writeToFile)
            {
                using (var fileStream = new FileStream(fileOutputPath, FileMode.Append))
                using (var writer = new StreamWriter(fileStream))
                {
                    await writer.WriteLineAsync(message);
                }
            }
        }
    }
}
