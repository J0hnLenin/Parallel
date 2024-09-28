using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static void Main()
    {
        List<int> S = new() { 100, 500, 1500};
        foreach (int size in S)
        {
            List<int> T = new() { 1, 2, 4, 8, 12, 16, 20 };
            foreach (int k in T)
            {
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = k
                };

                var A = GenerateRandomMatrix(size);
                var B = GenerateRandomMatrix(size);

                //PrintMatrix(A);
                //PrintMatrix(B);

                var sw1 = Stopwatch.StartNew();
                var resultRows = MultiplyMatricesByRows(A, B, parallelOptions);
                sw1.Stop();
                //PrintMatrix(resultRows);
                Console.WriteLine($"{size} {k} по строкам: {sw1.ElapsedMilliseconds / 1000.0} с");


                var sw2 = Stopwatch.StartNew();
                var resultColumns = MultiplyMatricesByColumns(A, B, parallelOptions);
                sw2.Stop();
                //PrintMatrix(resultColumns);
                Console.WriteLine($"{size} {k} по столбцам: {sw2.ElapsedMilliseconds / 1000.0} с");
            }
        }
        Console.ReadLine();
    }

    static int[,] GenerateRandomMatrix(int size)
    {
        var random = new Random();
        var matrix = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j] = random.Next(1, 10);
            }
        }
        return matrix;
    }

    static int[,] MultiplyMatricesByRows(int[,] A, int[,] B, ParallelOptions parallelOptions)
    {
        int size = A.GetLength(0);
        var result = new int[size, size];

        Parallel.For(0, size, parallelOptions, i =>
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    result[i, j] += A[i, k] * B[k, j];
                }
            }
        });

        return result;
    }

    static int[,] MultiplyMatricesByColumns(int[,] A, int[,] B, ParallelOptions parallelOptions)
    {
        int size = A.GetLength(0);
        var result = new int[size, size];
        for (int i = 0; i < size; i++)  
        {

            Parallel.For(0, size, parallelOptions, j => {
                for (int k = 0; k < size; k++)
                {
                    result[i, j] += A[i, k] * B[k, j];
                }
            });
        }

        return result;
    }

    static void PrintMatrix(int[,] matrix)
    {
        int size = matrix.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}