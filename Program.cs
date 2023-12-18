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
            for (int i = 0; i < 10000; i++)
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
            for (int i = 0; i < 10000; i++)
            {
                stopwatch.Start();
                json = JsonSerializer.Serialize(data, optJson);
                stopwatch.Stop();
            }
            Console.WriteLine($"JSON result string:{Environment.NewLine}{json}");
            Console.WriteLine($"JSON serialization time: {stopwatch.ElapsedMilliseconds}");

            File.WriteAllText("result.csv", csv);

            stopwatch.Reset();
            for (int i = 0; i < 10000; i++)
            {
                stopwatch.Start();
                F after = CsvSerializer<F>.Deserialize(File.ReadAllText("result.csv"), optCsv);
                stopwatch.Stop();
            }
            Console.WriteLine($"CSV deserialization time: {stopwatch.ElapsedMilliseconds}");

            File.WriteAllText("result.json", json);
            stopwatch.Reset();
            for (int i = 0; i < 10000; i++)
            {
                stopwatch.Start();
                F? after = JsonSerializer.Deserialize<F>(File.ReadAllText("result.json"), optJson);
                stopwatch.Stop();
            }
            Console.WriteLine($"JSON deserialization time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
