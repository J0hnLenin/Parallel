using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics.Metrics;
using System.Net.Security;

public class Task5
{
    public class Printer
    {
        public string document = "";
        public int id;
        Task printerTask;
        public Printer(int id) 
        {
            this.id = id;
            this.printerTask = new Task(() => Start());
            this.printerTask.Start();
        }
        void Start()
        {
            while (true)
            {
                if (document != "")
                {
                    Console.WriteLine("Printer {0} printing: {1}", id.ToString(), document);
                    Thread.Sleep(900);
                    document = "";
                }
            }
        }
    }
    public class User
    {
        public int id;
        Task userTask;
        public User(int id)
        {
            this.id = id;
            this.userTask = new Task(() => Start());
            this.userTask.Start();
        }
        void Start()
        {
            int documentNumber = id;
            while (true)
            {
                int printerId = request();
                Console.WriteLine("\n User {0} send document {1} to printer {2}", id.ToString(), documentNumber.ToString(), printerId.ToString());
                printers[printerId].document = string.Format("document {0} from user {1}", documentNumber.ToString(), id.ToString());
                while (printers[printerId].document != "")
                    Thread.Sleep(50);

                documentNumber += totalUsers;
                release(printerId);
                Thread.Sleep(5000);
            }
        }
    }
    static int request()
    {
        while (true)
        {
            for (int i = 0; i < totalPrinters; i++)
            {
                bool lockTaken = false;
                Monitor.TryEnter(printers[i], 10, ref lockTaken);
                if (lockTaken)
                    return i;
            }
            Thread.Sleep(100);
        }
    }
    static void release(int printerId)
    {
        Monitor.Exit(printers[printerId]);
    }
    const int totalPrinters = 2;
    const int totalUsers = 5;
    static Printer[] printers = new Printer[totalPrinters];
    static User[] users = new User[totalUsers];
    public static void Main5()
    {
        for (int i = 0; i<totalPrinters; i++)
        {
            printers[i] = new Printer(i);
        }
        for (int i = 0; i < totalUsers; i++)
        {
            users[i] = new User(i);
        }
        while (true)
        {
            Thread.Sleep(100);
        }
    }
}