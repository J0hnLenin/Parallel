using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Task1
{
    static SemaphoreSlim producerSemaphore = new SemaphoreSlim(0);
    static int consumersCount = 0;
    static readonly int totalConsumers = 5;
    static string buffer = "";
    static Mutex mutex = new Mutex();

    public class Consumer
    {
        int id;
        string mailBox = "|";
        public Consumer(int id) => this.id = id;

        public void Fit(string value)
        {
            mutex.WaitOne();
            mailBox = $"{mailBox}-{value}";
            Console.WriteLine($"Consumer {id}: {mailBox}");
            consumersCount++;
            if (consumersCount == totalConsumers)
            {
                producerSemaphore.Release();
            }
            mutex.ReleaseMutex();
        }
    }

    public class Producer
    {
        private int messageNumber = 0;

        public void FillBuffer()
        {
            buffer = $"message{++messageNumber}";
            Console.WriteLine($"Produced: {buffer}");
        }
    }

    public static void Main1()
    {
        var consumers = new Consumer[totalConsumers];
        for (int i = 0; i < totalConsumers; i++)
            consumers[i] = new Consumer(i);

        var producer = new Producer();
        int totalMessages = 6;
        for (int i = 0; i < totalMessages; i++)
        {
            consumersCount = 0;
            producer.FillBuffer();
            var tasks = new Task[totalConsumers];
            for (int j = 0; j < totalConsumers; j++)
            {
                int index = j;
                tasks[index] = Task.Run(() => consumers[index].Fit(buffer));
            }
            Task.WaitAll(tasks);
            producerSemaphore.Wait();
        }
    }
}