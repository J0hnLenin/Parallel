using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics.Metrics;
using System.Net.Security;

public class Task2
{
    static int bowlFood = 10;
    static Mutex bowlMutex = new Mutex();

    public class Сhick
    {
        int id;
        Mother mother;
        public Сhick(int id, Mother mother)
        {
            this.id = id;
            this.mother = mother;
        }

        public void Eat()
        {
            while (true)
            {
                bowlMutex.WaitOne();
                if (bowlFood == 0)
                {
                    mother.call = true;
                    Console.WriteLine($"Chick {id}: call mother!");
                }
                else
                {
                    bowlFood--;
                    Console.WriteLine($"Chick {id}: {bowlFood} food in bowl.");
                }
                bowlMutex.ReleaseMutex();
                Thread.Sleep(1000);
                // Наелся и спит
            }
        }
    }

    public class Mother
    {
        public bool call = false;
        public void Create() 
        {
            while (true)
            {
                if (call)
                {
                    bowlFood += 10;
                    call = false;
                    Console.WriteLine($"Mother : {bowlFood} food in bowl.");
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }

    public static void Main2()
    {
        var mother = new Mother();
        Task motherTask = Task.Run(() => mother.Create());
        
        int totalChiks = 5;
        var chiks = new Сhick[totalChiks];
        for (int i = 0; i < totalChiks; i++)
            chiks[i] = new Сhick(i, mother);

        var tasks = new Task[totalChiks];
        for (int j = 0; j < totalChiks; j++)
        {
            int index = j;
            tasks[index] = Task.Run(() => chiks[index].Eat());
        }
        while (true)
        {
            Thread.Sleep(100);
        }
    }
}