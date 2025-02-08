using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class ParallelSortingAnalysis
{
    // Class to store timing results
    class SortResult
    {
        public string Name { get; set; }
        public long Time { get; set; }
    }

    static void Main()
    {
        const int ITERATIONS = 100;
        const int LIST_SIZE = 1000;

        // Counters for fastest algorithm in each iteration
        var winCount = new Dictionary<string, int>
        {
            {"QuickSort", 0},
            {"BubbleSort", 0},
            {"SelectionSort", 0},
            {"InsertionSort", 0},
            {"LinqSort", 0}
        };

        // Store all timing results for statistical analysis
        var allTimings = new Dictionary<string, List<long>>
        {
            {"QuickSort", new List<long>()},
            {"BubbleSort", new List<long>()},
            {"SelectionSort", new List<long>()},
            {"InsertionSort", new List<long>()},
            {"LinqSort", new List<long>()}
        };

        Console.WriteLine($"Starting performance analysis over {ITERATIONS} iterations...\n");

        for (int iteration = 0; iteration < ITERATIONS; iteration++)
        {
            // Generate random data
            var random = new Random();
            var originalList = Enumerable.Range(0, LIST_SIZE)
                                       .Select(_ => random.Next(1, 100000))
                                       .ToList();

            var results = new List<SortResult>();
            var tasks = new List<Task<SortResult>>();

            // QuickSort (List.Sort())
            tasks.Add(Task.Run(() =>
            {
                var list = new List<int>(originalList);
                var sw = Stopwatch.StartNew();
                list.Sort();
                sw.Stop();
                return new SortResult { Name = "QuickSort", Time = sw.ElapsedTicks };
            }));

            // BubbleSort
            tasks.Add(Task.Run(() =>
            {
                var list = new List<int>(originalList);
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < list.Count - 1; i++)
                    for (int j = 0; j < list.Count - i - 1; j++)
                        if (list[j] > list[j + 1])
                            (list[j], list[j + 1]) = (list[j + 1], list[j]);
                sw.Stop();
                return new SortResult { Name = "BubbleSort", Time = sw.ElapsedTicks };
            }));

            // SelectionSort
            tasks.Add(Task.Run(() =>
            {
            var list = new List<int>(originalList);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < list.Count - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < list.Count; j++)
                    if (list[j] < list[minIndex])
                        minIndex = j;
                    if (minIndex != i)
                        (list[i], list[minIndex]) = (list[minIndex], list[i]);
                }
                sw.Stop();
                return new SortResult { Name = "SelectionSort", Time = sw.ElapsedTicks };
            }));

            // InsertionSort
            tasks.Add(Task.Run(() =>
            {
                var list = new List<int>(originalList);
                var sw = Stopwatch.StartNew();
                for (int i = 1; i < list.Count; i++)
                {
                    int key = list[i];
                    int j = i - 1;
                    while (j >= 0 && list[j] > key)
                    {
                        list[j + 1] = list[j];
                        j--;
                    }
                    list[j + 1] = key;
                }
                sw.Stop();
                return new SortResult { Name = "InsertionSort", Time = sw.ElapsedTicks };
            }));

            // LINQ Sort
            tasks.Add(Task.Run(() =>
            {
                var list = new List<int>(originalList);
                var sw = Stopwatch.StartNew();
                list = list.OrderBy(x => x).ToList();
                sw.Stop();
                return new SortResult { Name = "LinqSort", Time = sw.ElapsedTicks };
            }));

            // Wait for all tasks and collect results
            Task.WaitAll(tasks.ToArray());
            results.AddRange(tasks.Select(t => t.Result));

            // Find fastest sort for this iteration
            var fastest = results.OrderBy(r => r.Time).First();
            winCount[fastest.Name]++;

            // Store timings for statistical analysis
            foreach (var result in results)
            {
                allTimings[result.Name].Add(result.Time);
            }

            // Show progress every 100 iterations
            if ((iteration + 1) % 100 == 0)
                Console.WriteLine($"Completed {iteration + 1} iterations...");
        }

        Console.WriteLine("\nPerformance Analysis Results:");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("\nNumber of times each algorithm was fastest:");
        foreach (var pair in winCount.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{pair.Key}: {pair.Value} times ({(double)pair.Value / ITERATIONS * 100:F2}%)");
        }

        Console.WriteLine("\nAverage execution time (ticks):");
        foreach (var pair in allTimings)
        {
            double avg = pair.Value.Average();
            double stdDev = Math.Sqrt(pair.Value.Average(v => Math.Pow(v - avg, 2)));
            Console.WriteLine($"{pair.Key}:");
            Console.WriteLine($"  Average: {avg:F2}");
            Console.WriteLine($"  Std Dev: {stdDev:F2}");
        }
    }
}