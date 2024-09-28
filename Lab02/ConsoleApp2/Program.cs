using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static void Main()
    {
        List<double> E = new() { 0.0001, 0.00001, 0.000001, 0.0000001, 0.00000001 };
        foreach (double esp in E)
        {
            List<int> T = new() { 1, 2, 4, 8, 12, 16, 20 };
            foreach (int k in T)
            {
                double a = 0.0;
                double b = Math.PI;

                var sw = Stopwatch.StartNew();
                double result = Integrate(a, b, esp, k);
                sw.Stop();

                Console.WriteLine($"{esp} {k} {sw.ElapsedMilliseconds/1000.0}");
                Console.WriteLine($"Приближённое значение интеграла: {result}");
            }
        }
        Console.ReadLine();
    }

    static double f(double x)
    {
        return Math.Sin(x) + Math.Cos(x) + x;
    }

    static double Integrate(double a, double b, double esp, int k)
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = k
        };

        double deltha = (b - a) / (k * 1.0);
        double sum = 0.0;
        Parallel.For(0, k, parallelOptions, i =>
        {
            double h = deltha;
            double l = a + i * deltha;
            double r = l + deltha;

            double prevArea = (deltha / 2) * (f(l) + f(r));
            int n = 1;
            double area = 0.0;

            while (true)
            {
                n *= 2;
                h /= 2;
                area = 0.0;

                for (int w = 0; w < n; w++)
                {
                    double x = l + w * h;
                    area += f(x);
                }

                area = (r - l) * area / n;
                if (Math.Abs(area - prevArea) < esp)
                    break;

                prevArea = area;
            }
            //Console.WriteLine($"{k} {l} {r} {sum} {area}");
            sum += area;

        });
        return sum;

    }

}