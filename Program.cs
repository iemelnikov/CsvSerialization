using System.Diagnostics;
using System.Text.Json;

namespace CsvSerialization
{
    public class Program
    {
        static void Main(string[] args)
        {
            F data = F.Get();
            var optCsv = new CsvSerializerOptions
            {
                Separator = ';'
            };
            string csv = string.Empty;
            var stopwatch = new Stopwatch();
            int iterCount = 10000;
            for (int i = 0; i < iterCount; i++)
            {
                stopwatch.Start();
                csv = CsvSerializer<F>.Serialize(data, optCsv);
                stopwatch.Stop();
            }
            Console.WriteLine($"CSV result string:{Environment.NewLine}{csv}");
            Console.WriteLine($"CSV serialization time: {stopwatch.ElapsedMilliseconds}");

            var optJson = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IncludeFields = true
            };
            stopwatch.Reset();
            string json = string.Empty;
            for (int i = 0; i < iterCount; i++)
            {
                stopwatch.Start();
                json = JsonSerializer.Serialize(data, optJson);
                stopwatch.Stop();
            }
            Console.WriteLine($"JSON result string:{Environment.NewLine}{json}");
            Console.WriteLine($"JSON serialization time: {stopwatch.ElapsedMilliseconds}");

            string csvFileName = "result.csv";
            File.WriteAllText(csvFileName, csv);
            string csvFileContent = File.ReadAllText(csvFileName);
            stopwatch.Reset();
            for (int i = 0; i < iterCount; i++)
            {
                stopwatch.Start();
                F after = CsvSerializer<F>.Deserialize(csvFileContent, optCsv);
                stopwatch.Stop();
            }
            Console.WriteLine($"CSV deserialization time: {stopwatch.ElapsedMilliseconds}");

            string jsonFileName = "result.json";
            File.WriteAllText(jsonFileName, json);
            string jsonFileContent = File.ReadAllText(jsonFileName);
            stopwatch.Reset();
            for (int i = 0; i < iterCount; i++)
            {
                stopwatch.Start();
                F? after = JsonSerializer.Deserialize<F>(jsonFileContent, optJson);
                stopwatch.Stop();
            }
            Console.WriteLine($"JSON deserialization time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
