using System;
using System.Collections.Generic;
using System.Threading.Tasks;


static class Utils
{
    static Random random = new Random();
    public static double rand(double value, double error)
    {
        return Math.Round(random.NextDouble() * 2 * error + value - error, 2);
    }

    public static double min(params double?[] values)
    {
        double min = double.MaxValue;
        foreach (double? value in values)
        {
            if (value != null && value < min) min = Convert.ToDouble(value);
        }
        return min;
    }
}

public class Task
{
  
    public double createTime;

    public Task(double createTime)
    {
        this.createTime = createTime;

    }
}

public class Computer
{
    public int id;
    public Queue<Task> queue;
    public double time = 0;
    public double workTime = 0;
    public double downTime = 0;
    public double processingTime;
    public double processingTimeError;
    public double? currentProcessingTime;
    public int completedTaskCount = 0;
    private double queueCountSum = 0;
    private double meanQueueCount = 0;
    private double meanWorkTimeTask = 0;
    private double processCount = 0;
    private double waitingTimeSum = 0;
    private double meanWaitingTime = 0;
    private double processingTimeSum = 0;
 

    public Computer(int id, double processingTime, double processingTimeError)
    {
        this.id = id;
        queue = new Queue<Task>();
        this.processingTime = processingTime;
        this.processingTimeError = processingTimeError;
    }

    public bool IsEmpty()
    {
        return queue.Count == 0;
    }

    public void AddTask()
    {
        Task task = new Task(workTime + downTime);
        Enqueue(task);
        if (currentProcessingTime == null) currentProcessingTime = Utils.rand(processingTime, processingTimeError);

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
            meanWorkTimeTask = workTime / completedTaskCount;
            processingTimeSum += processingTime;
            var createTime = queue.Peek().createTime;
            waitingTimeSum += workTime + downTime - createTime - Convert.ToDouble(currentProcessingTime);
            meanWaitingTime = waitingTimeSum / completedTaskCount;
            currentProcessingTime = null;
            return queue.Dequeue();

        }
        else
        {
            return null;
        }
    }


    public List<Task> Process(double progress)
    {
        List<Task> tasks = new List<Task>();

        if (IsEmpty()) {
            downTime += progress;
            return tasks;
        }
        workTime += progress;
        time += progress;

        // TODO: Fix calculation (сonsider progress)
      
        queueCountSum += queue.Count;
        processCount++;
        meanQueueCount = queueCountSum / processCount;

        while (time > 0 && !IsEmpty())
        {
          

            if (currentProcessingTime == null) currentProcessingTime = Utils.rand(processingTime, processingTimeError);

            // TODO: Handle time
            if (currentProcessingTime < (time ))
            {
                time -= Convert.ToDouble(currentProcessingTime);
                tasks.Add(Dequeue());
            }
            else
            {
                break;
            }
        }


        if (IsEmpty()) time = 0;

        return tasks;
    }



    public string GetTempStats()
    {
        string str = "";

        str += "ЭВМ" + id + ": ";
        str += "Очередь - " + queue.Count + ",  ";
        str += "Время обработки - " + Math.Round(currentProcessingTime ?? 0, 2) + " м.,  ";
        str += "Процесс - " + Math.Round(time,2) + " м.\n";

        return str;
    }

    public string GetStats()
    {
        string str = "";

        str += "ЭВМ" + id + ":\n ";
        str += "Время работы - " + Math.Round(workTime, 2) + "\n ";
        str += "Время простоя - " + Math.Round(downTime,2) + "\n ";
        str += "Коэффициент загрузки - " + Math.Round(workTime / (workTime + downTime > 0 ? workTime + downTime : 1), 2) + "\n ";
        str += "Средняя длина очереди - " + Math.Round(meanQueueCount, 2) + "\n ";
        str += "Среднее время ожидания заявки в очереди - " + Math.Round(meanWaitingTime, 2) + "\n ";
        str += "Среднее время обработки задания - " +  Math.Round(meanWorkTimeTask, 2) + "\n ";
        str += "Количество обработанных - " + completedTaskCount + "\n\n";

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
    public double timePerStep;

    public ComputingSystemSettings(double taskInterval, double taskIntervalError, double prob1, double prob2, double processingTime1, double processingTime1Error, double processingTime2, double processingTime2Error, double processingTime3, double processingTime3Error, int maxCount, double probMove2, double timePerStep)
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
        this.timePerStep = timePerStep;
    }
}

public class ComputingSystem
{

    public ComputingSystemSettings settings;
    public Computer computer1;
    public Computer computer2;
    public Computer computer3;

    public double time = 0;
    public double workTime = 0;
    public double downTime = 0;
    public double taskInterval = 0;
    public int completedTaskCount = 0;
    public int taskCount;

    public ComputingSystem(ComputingSystemSettings settings)
    {
        this.settings = settings;
        computer1 = new Computer(1, settings.processingTime1, settings.processingTime1Error);
        computer2 = new Computer(2, settings.processingTime2, settings.processingTime2Error);
        computer3 = new Computer(3, settings.processingTime3, settings.processingTime3Error);
        setTaskInterval();
    }

    private void setTaskInterval()
    {
        taskInterval = Utils.rand(settings.taskInterval, settings.taskIntervalError);
    }
    public void Process() => Process(settings.timePerStep);
    public void Process(double progress)
    {

        if (completedTaskCount < settings.maxTasks)
        {

            if (computer1.IsEmpty() && computer2.IsEmpty() && computer3.IsEmpty())
            {
                downTime += progress;
            }
            else
            {
                workTime += progress;
            }
        }

        var tasks1 = computer1.Process(progress);

        for (int i = 0; i < tasks1.Count; i++)
        {
            double taskType = Utils.rand(0.5, 0.5);

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

        if (taskCount < settings.maxTasks)
        {
            time += progress;
            if(taskInterval <= time + 0.001)
            {
                while (taskInterval <= (time + 0.001) && taskCount < settings.maxTasks)
                {
                    time -= taskInterval;

                    double taskType = Utils.rand(0.5, 0.5);

                    if (taskType < settings.prob1)
                    {
                        computer1.AddTask();
                    }
                    else if (taskType < settings.prob1 + settings.prob2)
                    {
                        computer2.AddTask();
                    }
                    else
                    {
                        computer3.AddTask();
                    }

                    taskCount++;
                    setTaskInterval();
                }
            }
           
        }

      


    }

    public void InstantlyFinish()
    {
        while (completedTaskCount < settings.maxTasks)
        {
            if(computer1.IsEmpty() &&  computer2.IsEmpty() && computer3.IsEmpty())
            {
                Process(taskInterval);
            } 
            else
            {
                var min = Utils.min(computer1.currentProcessingTime, computer2.currentProcessingTime, computer3.currentProcessingTime);
                Process(min);
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

        str += "Временные параметры: \n";
        str += computer1.GetTempStats();
        str += computer2.GetTempStats();
        str += computer3.GetTempStats();
        str += "\nВыходные параметры: \n";
        str += computer1.GetStats();
        str += computer2.GetStats();
        str += computer3.GetStats();
        str += "Система:\n";
        str += "Время работы: " + Math.Round(workTime, 2) + " м.\n";
        str += "Время простоя: " + Math.Round(downTime, 2) + " м.\n";
        double meanWorkTime = (computer1.workTime + computer2.workTime + computer3.workTime) / (computer1.completedTaskCount + computer2.completedTaskCount + computer3.completedTaskCount);
        str += "Среднее время обработки задания - " + (meanWorkTime > 0 && !double.IsInfinity(meanWorkTime) ? Math.Round(meanWorkTime, 2) : 0) + "\n";
        str += "Количество выполненных заданий: " + completedTaskCount + "\n";

        return str;
    }

}