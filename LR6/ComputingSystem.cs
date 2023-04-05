using System;
using System.Collections.Generic;
using System.Threading.Tasks;


static class Rand
{   
    static Random random = new Random();
    public static double get(double value, double error)
    {
        return Math.Round(random.NextDouble() * 2 * error + value - error, 2);
    }
}

public class Task
{
    public double? timeLeft;

    public Task(double? timeLeft)
    {
        this.timeLeft =  timeLeft;
    }
}

public class Computer
{
    public int id;
    public Queue<Task> queue;
    public double time;
    public double processingTime;
    public double processingTimeError;
    public int completedTaskCount;

    private Random random;

   

    public Computer(int id, double processingTime, double processingTimeError)
    {
        this.id = id;
        queue = new Queue<Task>();
        time = 0;
        this.processingTime = processingTime;
        this.processingTimeError = processingTimeError;

        random = new Random();
    }

    public bool IsEmpty()
    {
        return queue.Count == 0;
    }

    public void AddTask()
    {
        Task task = new Task(null);
        Enqueue(task);
    }


    public List<Task> Process(double progress)
    {
        List<Task> tasks = new List<Task>();

        if (IsEmpty()) return tasks;

        time += progress;

     

        while (time > 0 && !IsEmpty())
        {
            Task task = queue.Peek();

            if (task.timeLeft == null) task.timeLeft = Rand.get(processingTime, processingTimeError);

            if (task.timeLeft < time)
            {
                time -= Convert.ToDouble(task.timeLeft) ;
                tasks.Add(Dequeue());
            } else
            {
                break;
            }
        }

        time = Math.Round(time, 2);

        if (IsEmpty()) time = 0;

        return tasks;
    }

    private void Enqueue(Task task)
    {
        queue.Enqueue(task);
    }

    private Task Dequeue()
    {
        if (queue.Count > 0)
        {

            completedTaskCount += 1;
            return queue.Dequeue();
            
        }
        else
        {
            return null;
        }
    }

    private Task Peek()
    {
        if (queue.Count > 0)
        {
            return queue.Peek();
        }
        else
        {
            return null;
        }
    }

    public void Restart()
    {
        queue.Clear();
        time = 0;
        completedTaskCount = 0;
    }

    override
    public string ToString()
    {
        string str = "";

        str += "Computer" + id + ":" + ",  ";
        str += "Tasks - " + queue.Count + ",  ";
        str += "taskTimeLeft - " + (Peek()?.timeLeft ?? 0) + " м.,  ";
        str += "time - " + time + "м.\n";

        return str;
    }

}

public class ComputingSystemSettings
{
    public double taskInterval;
    public double taskIntervalError;
    public double prob1;
    public double prob2;
    public double processingTime1;
    public double processingTime1Error;
    public double processingTime2;
    public double processingTime2Error;
    public double processingTime3;
    public double processingTime3Error;
    public int maxTasks;
    public double probMove2;

    public ComputingSystemSettings (double taskInterval, double taskIntervalError, double prob1, double prob2, double processingTime1, double processingTime1Error, double processingTime2, double processingTime2Error, double processingTime3, double processingTime3Error, int maxCount, double probMove2)
    {
        this.taskInterval = taskInterval;
        this.taskIntervalError = taskIntervalError;
        this.prob1 = prob1;
        this.prob2 = prob2;
        this.processingTime1 = processingTime1;
        this.processingTime1Error = processingTime1Error;
        this.processingTime2 = processingTime2;
        this.processingTime2Error = processingTime2Error;
        this.processingTime3 = processingTime3;
        this.processingTime3Error = processingTime3Error;
        this.maxTasks = maxCount;
        this.probMove2 = probMove2;
    }
}

public class ComputingSystem
{

    public ComputingSystemSettings settings;
    public Computer computer1;
    public Computer computer2;
    public Computer computer3;
 
    private  Random random;
    public double allTime;
    public double time;
    public double timeLeft;
    public int completedTaskCount;
    public int taskCount;



    public ComputingSystem(ComputingSystemSettings settings)
    {
        this.settings = settings;
       computer1 = new Computer(1, settings.processingTime1, settings.processingTime1Error);
        computer2 = new Computer(2, settings.processingTime2, settings.processingTime2Error);
        computer3 = new Computer(3, settings.processingTime3, settings.processingTime3Error);
        time = 0;
        timeLeft = 0;
        allTime = 0;
        taskCount = 0;
        completedTaskCount = 0;
        random = new Random();
    }

    public void Process(double progress)
    {
        if(taskCount < settings.maxTasks)
        {
            time += progress;
            while (timeLeft < time && taskCount < settings.maxTasks) { 
                time -= timeLeft;

                double taskType = random.NextDouble();

                if (taskType < settings.prob1)
                {
                    computer1.AddTask();
                    taskCount++;
                }
                else if (taskType < settings.prob1 + settings.prob2)
                {
                    computer2.AddTask();
                    taskCount++;
                }
                else
                {
                    computer3.AddTask();
                    taskCount++;
                }

                timeLeft = Rand.get(settings.taskInterval, settings.taskIntervalError);
            }
        }

        if (completedTaskCount < settings.maxTasks)
        {
            allTime += progress;
            allTime = Math.Round(allTime, 2);
        }

        var tasks1 = computer1.Process(progress);

        for(int i = 0; i < tasks1.Count; i++)
        {
            double taskType = random.NextDouble();

            if (taskType < settings.probMove2)
            {
                computer2.AddTask();
            }
            else
            {
                computer3.AddTask();
            }
        }

        var tasks2 = computer2.Process(progress);
        var tasks3 = computer3.Process(progress);

        completedTaskCount += tasks2.Count + tasks3.Count;


    }

    public void Restart()
    {
        computer1.Restart();
        computer2.Restart();
        computer3.Restart();
        time = 0;
        taskCount = 0;
        completedTaskCount = 0;
        allTime = 0;
    }

    public void InstantlyFinish()
    {
        //Process(99999);
        while (true)
        {
            Process(0.1);
            if (completedTaskCount == settings.maxTasks)
            {
                break;
            }
        }
    }

    public void loadNewParameters(ComputingSystemSettings settings)
    {
        this.settings = settings;

        computer1.processingTime = settings.processingTime1;
        computer1.processingTimeError = settings.processingTime1Error;
        computer2.processingTime = settings.processingTime2;
        computer2.processingTimeError = settings.processingTime2Error;
        computer3.processingTime = settings.processingTime3;
        computer3.processingTimeError = settings.processingTime3Error;
    }

    override
    public string ToString()
    {
        string str = "";

        str += computer1.ToString();
        str += computer2.ToString();
        str += computer3.ToString();
        str += "Сomputer1 Completed tasks: " + computer1.completedTaskCount + "\n";
        str += "Сomputer2 Completed tasks: " + computer2.completedTaskCount + "\n";
        str += "Сomputer3 Completed tasks: " + computer3.completedTaskCount + "\n";
        str += "Completed tasks: " + completedTaskCount + "\n";
        str += "All work time: " + allTime + "м.\n";


        return str;
    }

}