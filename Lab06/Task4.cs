using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics.Metrics;
using System.Net.Security;

public class Task4
{
    public class Trolley
    {
        public string state = "wait left";
        public int position = 0;
        public int truckLen = 10;
        
        public void Start()
        {
            while (true)
            {
                switch (state)
                {
                    case "go right":
                        if (position < truckLen)
                            position++;
                        else
                        {
                            state = "exit right";
                        }
                        break;
                    case "go left":
                        if (position > 0)
                            position--;
                        else
                        {
                            state = "exit left";
                        }
                        break;
                    case "wait left":
                        if (passengersInTrolley.Count == totalPlases)
                            state = "go right";
                        break;
                    case "wait right":
                        if (passengersInTrolley.Count == totalPlases)
                            state = "go left";
                        break;
                    case "exit left":
                        if (passengersInTrolley.Count == 0)
                            state = "wait left";
                        break;
                    case "exit right":
                        if (passengersInTrolley.Count == 0)
                            state = "wait right";
                        break;
                }
                Thread.Sleep(450);
            }
        }
        
    }
    class Passenger
    {
        public int id;
        public Task task;
        string state = "";
        public Passenger(int id, string state)
        {
            this.id = id;
            this.state = state;
            this.task = new Task(() => Start());
            this.task.Start();
        }
        public void Start() 
        {
            while(true)
            {
                if (state == trolley.state)
                {
                    Monitor.Enter(trolley);
                    // + Критическая зона
                    switch (state)
                    {
                        case "wait left":
                            if (passengersInTrolley.Count < totalPlases)
                            {
                                leftQueue.Remove(this);
                                passengersInTrolley.Add(this);
                                state = "exit right";
                            }
                            break;
                        case "wait right":
                            if (passengersInTrolley.Count < totalPlases)
                            {
                                rightQueue.Remove(this);
                                passengersInTrolley.Add(this);
                                state = "exit left";
                            }
                            break;
                        case "exit left":
                            passengersInTrolley.Remove(this);
                            leftQueue.Add(this);
                            state = "wait left";
                            break;
                        case "exit right":
                            passengersInTrolley.Remove(this);
                            rightQueue.Add(this);
                            state = "wait right";
                            break;
                    }
                    // - Критическая зона
                    Monitor.Exit(trolley);
                }
                Thread.Sleep(900 + 10*id);
            }
        }
    }
    static int totalPlases = 3;
    static Trolley trolley = new Trolley();
    static List<Passenger> leftQueue = new List<Passenger>();
    static List<Passenger> rightQueue = new List<Passenger>();
    static List<Passenger> passengersInTrolley = new List<Passenger>();
    static public void printState()
    {
        Monitor.Enter(trolley);
        // + Критическая зона
        string s = new string(' ', 50 - leftQueue.Count); ;
        foreach (Passenger passenger in leftQueue)
        {
            s += passenger.id.ToString();
        }
        s += "=";
        s = s.Insert(s.Length, new string('.', trolley.position));
        s += "o\\";
        foreach (Passenger passenger in passengersInTrolley)
        {
            s += passenger.id.ToString();
        }
        s = s.Insert(s.Length, new string('_', totalPlases - passengersInTrolley.Count));
        s += "/o";
        s = s.Insert(s.Length, new string('.', trolley.truckLen - trolley.position)) + "=";
        foreach (Passenger passenger in rightQueue)
        {
            s += passenger.id.ToString();
        }
        s = s.Insert(s.Length, new string('\n', 14));
        Console.WriteLine(s);
        // - Критическая зона
        Monitor.Exit(trolley);
    }
    public static void Main4()
    {
        int totalPassengers = 10;

        for (int i = 0;i < totalPassengers; i++)
        {
            if (i < totalPassengers/2)
                leftQueue.Add(new Passenger(i, "wait left"));
            else 
                rightQueue.Add(new Passenger(i, "wait right"));
        }


        var trolleyTask = new Task(() => trolley.Start());
        trolleyTask.Start();
        Console.WriteLine(new string('\n', 14));
        while (true)
        {
            printState();
            Thread.Sleep(300);
        }
    }
}