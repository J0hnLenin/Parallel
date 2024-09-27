using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

delegate void MethodDelegate(int[,] a, int N);

class Program
{
    static void Main()
    {
        int M = 1000;
        int N = 6000;
        int[,] a = new int[N, N];

        TestMethod(a, N, M, Method1);
        TestMethod(a, N, M, Method2);
        TestMethod(a, N, M, Method3);
        TestMethod(a, N, M, Method4);

    }

    static void Method1(int[,] a, int N)
    {
        for (int j = 0; j < N; j++)
            for (int i = 0; i < N; i++)
                a[j, i] = i / (i + j + 1);
    }
    static void Method2(int[,] a, int N)
    {
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                a[j, i] = i / (i + j + 1);
    }
    static void Method3(int[,] a, int N)
    {
        for (int i = N-1; i > 0; i--)
            for (int j = N-1; j > 0; j--)
                a[j, i] = i / (i + j + 1);
    }
    static void Method4(int[,] a, int N)
    {
        for (int j = N-1; j > 0; j--)
            for (int i = N- 1; i > 0; i--)
                a[j, i] = i / (i + j + 1);
    }

    static void TestMethod(int[,] a, int N, int M, MethodDelegate method)
    {
        double minn = 1000000.0;
        double maxx = 0.0;
        double avg = 0.0;

        Stopwatch all_sw = Stopwatch.StartNew();

        for (int k = 0; k < M; k++)
        {
            Stopwatch sw = Stopwatch.StartNew();

            method.Invoke(a, N);

            sw.Stop();

            double ans =  sw.ElapsedMilliseconds / 1000.0;

            if (ans < minn)
                minn = ans;
            if (ans > maxx)
                maxx = ans;
            if (k % 100 == 0)
                Console.WriteLine(k);
        }
        all_sw.Stop();
        avg = all_sw.ElapsedMilliseconds / 1000.0 / M;

        Console.WriteLine("{0} {1} {2}", minn, maxx, avg);
        
    }
}