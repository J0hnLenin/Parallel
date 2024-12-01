using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    const int ARRAY_SIZE = 5000;
    const int K = 20; // Количество потоков
    const int D = 50; // Длинна диапазона заполнения одного потока
    //const int D = ARRAY_SIZE / K; // Длинна диапазона наивное заполнение
    const double M = 1000.0; // Количество замеров

    static double[] A = new double[ARRAY_SIZE];
    static double[] C = new double[ARRAY_SIZE];
    static double[] B = new double[ARRAY_SIZE];

    static Random random = new Random();

    static int currentIndex = 0;
    static object lockObj = new object();
    static ManualResetEventSlim isThreadAvailable = new ManualResetEventSlim(true);

    static void FillArray(double[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.Next(1, 101);
        }
    }

    static void CalculateProduct(int left, int right)
    {
        for (int i = left; i < right; i++)
        {
            double summ = 0;
            for (int j = 0; j <= i; j++)
            {
                summ += Math.Pow(A[j], 1.789) + Math.Pow(C[j], 1.789);
            }
            B[i] = summ;
        }
        
    }

    static void FillArrayNaively(int id)
    {
        int l, r;
        l = id * D;
        r = l + D;
        
        CalculateProduct(l, r);
        //Console.WriteLine("Поток {0} заполнил диапазон {1} {2}", id, l, r);
        
    }

    static void FillArray(int id)
    {
        while (true)
        {
            int l, r;

            lock (l)
            {
                if (currentIndex >= ARRAY_SIZE) return;

                l = currentIndex;
                r = Math.Min(l + D, ARRAY_SIZE);
                currentIndex = r;
                isThreadAvailable.Set();
            }

            CalculateProduct(l, r);
            //Console.WriteLine("Поток {0} заполнил диапазон {1} {2}", id, l, r);
        }
    }

    static void Main()
    {
        FillArray(A);
        FillArray(C);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int t=0; t<M; t++)
        {
            Thread[] threads = new Thread[K];
            for (int i = 0; i < K; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() => FillArray(threadIndex));
                threads[i].Start();
            }

            for (int i = 0; i < K; i++)
            {
                threads[i].Join();
            }

        }
        sw.Stop();
        
        Console.WriteLine("{0}", sw.ElapsedMilliseconds / 1000.0 / M);
        Console.ReadLine();

    }
}