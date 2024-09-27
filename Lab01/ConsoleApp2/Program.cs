using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Random random = new Random();

        for(int arraySize=1000; arraySize<=26000; arraySize+= 5000)
        {
            double[] a = new double[arraySize];
            double[] b = new double[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                a[i] = random.Next(1, 1000);
                b[i] = random.Next(1, 1000);
            }


            double[] array = new double[arraySize];

            for (int k = 0; k < 20; k++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var op = new ParallelOptions();
                op.MaxDegreeOfParallelism = 16;
                Parallel.For(0, k, op, threadId =>
                {

                    for (int i = threadId; i < arraySize; i += k)
                    {
                        double summ = 0;
                        for (int j = 0; j <= i; j++)
                        {
                            summ += Math.Pow(a[j], 1.789) + Math.Pow(b[j], 1.789);
                        }
                        array[i] = summ;

                    }

                });
                sw.Stop();
                Console.WriteLine("{0} {1} {3}", arraySize, k, sw.ElapsedMilliseconds / 1000.0);
            }
        }
        

    }
}