using System;
using System.Collections.Concurrent;

namespace TTWebClientRobot.Statistic
{
    public class Benchmark
    {
        public string Name { get; private set; }
        public double AvgTime { get { return (Iterations > 0.0) ? (TotalTime / Iterations) : 0.0; } }
        public double MinTime { get; private set; }
        public double MaxTime { get; private set; }
        public double TotalTime { get; private set; }
        public double Iterations { get; private set; }

        public Benchmark(string name)
        {
            Name = name;
            MinTime = double.MaxValue;
            MaxTime = double.MinValue;
        }

        public void Update(double time)
        {
            MinTime = (time < MinTime) ? time : MinTime;
            MaxTime = (time > MaxTime) ? time : MaxTime;
            TotalTime += time;
            Iterations++;
        }

        public void PrintStatistic()
        {
            Console.WriteLine("Benchmark name: " + Name);
            Console.WriteLine("Iterations: " + Iterations);
            Console.WriteLine("Avg time per iteration: " + AvgTime.ToString("0.000") + " ms");
            Console.WriteLine("Min time per iteration: " + MinTime.ToString("0.000") + " ms");
            Console.WriteLine("Max time per iteration: " + MaxTime.ToString("0.000") + " ms");
        }
    }
}
