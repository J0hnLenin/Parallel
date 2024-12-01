using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics.Metrics;
using System.Net.Security;

public class Task3
{
    static int bowlFood = 10;
    static Mutex bowlMutex = new Mutex();

    public class Bee
    {
        int id;
        public Bee(int id)
        {
            this.id = id;
        }

        public void Work()
        {
            while (true)
            {
                bowlMutex.WaitOne();
                bowlFood++;
                Console.WriteLine($"Bee {id}: {bowlFood} food in bowl.");
                bowlMutex.ReleaseMutex();
                Thread.Sleep(1250);
            }
        }
    }

    public class Bear
    {
        public void WakeUp() 
        {
            while (true)
            {
                bowlMutex.WaitOne();
                bowlFood = 0;
                Console.WriteLine($"\nBear: {bowlFood} food in bowl.");
                bowlMutex.ReleaseMutex();
                Thread.Sleep(2000);
            }
        }
    }

    public static void Main3()
    {
        var bear = new Bear();
        Task bearTask = Task.Run(() => bear.WakeUp());
        
        int totalBee = 5;
        var bee = new Bee[totalBee];
        for (int i = 0; i < totalBee; i++)
            bee[i] = new Bee(i);

        var tasks = new Task[totalBee];
        for (int j = 0; j < totalBee; j++)
        {
            int index = j;
            tasks[index] = Task.Run(() => bee[index].Work());
        }
        while (true)
        {
            Thread.Sleep(100);
        }
    }
}