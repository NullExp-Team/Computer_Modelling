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
    public double? processingTime;

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
    public double deadTime = 0;
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

    public void AddTask(Task task)
    {
        Enqueue(task);
        if (currentProcessingTime == null) currentProcessingTime = Utils.rand(processingTime, processingTimeError);
    }
    public void AddTask()
    {
        Task task = new Task(workTime + deadTime);
        Enqueue(task);
        if (currentProcessingTime == null) currentProcessingTime = Utils.rand(processingTime, processingTimeError);

    }

    private void Enqueue(Task task)
    {
        queue.Enqueue(task);
    }

    private Task CompleteTask()
    {
        if (queue.Count > 0)
        {
            completedTaskCount += 1;
            meanWorkTimeTask = workTime / completedTaskCount;
            processingTimeSum += processingTime;
            var task = queue.Dequeue();
            var createTime = task.createTime;
            var procTime = (task.processingTime ?? 0);
            task.processingTime = procTime + currentProcessingTime;
            waitingTimeSum += workTime + deadTime - createTime - Convert.ToDouble(currentProcessingTime);
            meanWaitingTime = waitingTimeSum / completedTaskCount;

            currentProcessingTime = null;
            return task;
        }
        else
        {
            return null;
        }
    }

    public List<Task> Process(double progress)
    {
        List<Task> tasks = new List<Task>();

        if (IsEmpty())
        {
            deadTime += progress;
            return tasks;
        }

        workTime += progress;
        time += progress;

        queueCountSum += queue.Count;
        processCount++;
        meanQueueCount = queueCountSum / processCount;

        while (time > 0 && !IsEmpty())
        {
            if (currentProcessingTime == null) 
                currentProcessingTime = Utils.rand(processingTime, processingTimeError);

            if (currentProcessingTime < (time))
            {
                time -= Convert.ToDouble(currentProcessingTime);
                tasks.Add(CompleteTask());
            }
            else
                break;
        }

        if (IsEmpty()) time = 0;

        return tasks;
    }

    public string GetTempStats()
    {
        string str = "";

        str += "ЭВМ " + id + ": " + "\n";
        str += "Очередь - " + queue.Count + "\n"; 
        str += "Время обработки - " + Math.Round(currentProcessingTime ?? 0, 2) + " мин.  " + "\n"; ;

        return str;
    }

    public double GetTimeComp()
    {
        return Math.Round(currentProcessingTime ?? 0, 2);
    }

    public string CheckEnding(int count)
    {
        string taskWord;

        if (count % 10 == 1 && count % 100 != 11)
        {
            taskWord = "задание.";
        }

        else if ((count % 10 >= 2 && count % 10 <= 4) && !(count % 100 >= 12 && count % 100 <= 14))
        {
            taskWord = "задания.";
        }
        else
        {
            taskWord = "заданий.";
        }

        return taskWord;
    }

    public string GetStats()
    {
        string str = "";

        str += "ЭВМ " + id + ":\n ";
        str += "Время работы - " + Math.Round(workTime, 2) + " мин. \n ";
        str += "Время простоя - " + Math.Round(deadTime, 2) + " мин. \n ";
        str += "Занятость - " + Math.Round(workTime / (workTime + deadTime > 0 ? workTime + deadTime : 1), 2) * 100 + "% " + "\n ";
        //str += "Средняя длина очереди - " + Math.Round(meanQueueCount, 2) + "\n ";
        str += "Среднее время ожидания задания в очереди - " + Math.Round(meanWaitingTime, 2) + " мин. \n ";
        str += "Среднее время обработки задания - " + Math.Round(meanWorkTimeTask, 2) + " мин. \n ";

        str += "Количество обработанных заданий - " + completedTaskCount + " " + CheckEnding(completedTaskCount) + "\n\n";

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
    public double deadTime = 0;
    public double taskInterval = 0;
    public int completedTaskCount = 0;
    public int taskCount;

    private double meanTimeToCompleteSum = 0;
    private double meanTimeToComplete = 0;

    private double meanPresenceTimeSum = 0;
    private double meanPresenceTime = 0;

    private double meanChannelLoadSum = 0;
    private double meanChannelLoad = 0;

    private double meanTaskCountSum = 0;
    private double meanTaskCount = 0;

    private double processCount = 0;

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
        processCount++;

        if (completedTaskCount < settings.maxTasks)
        {

            if (computer1.IsEmpty() && computer2.IsEmpty() && computer3.IsEmpty())
                deadTime += progress;
            else
                workTime += progress;
        }

        var tasks1 = computer1.Process(progress);

        foreach (Task task in tasks1)
        {
            double taskType = Utils.rand(0.5, 0.5);

            if (taskType < settings.probMove2)
                computer2.AddTask(task);
            else
                computer3.AddTask(task);
        }

        var tasks2 = computer2.Process(progress);
        var tasks3 = computer3.Process(progress);

        completedTaskCount += tasks2.Count + tasks3.Count;
        
        tasks2.ForEach((task) => { 
            meanTimeToCompleteSum += task.processingTime ?? 0;
            meanPresenceTimeSum += workTime + deadTime - task.createTime;
        });
        tasks3.ForEach((task) => { 
            meanTimeToCompleteSum += task.processingTime ?? 0;
            meanPresenceTimeSum += workTime + deadTime - task.createTime;
        });

        if (completedTaskCount > 0)
        {
            meanTimeToComplete = meanTimeToCompleteSum / completedTaskCount;
            meanPresenceTime = meanPresenceTimeSum / completedTaskCount;
        }

        meanChannelLoadSum += (computer1.IsEmpty() ? 0 : 1) + (computer2.IsEmpty() ? 0 : 1) + (computer3.IsEmpty() ? 0 : 1);
        meanChannelLoad = meanChannelLoadSum / processCount;

        meanTaskCountSum += computer1.queue.Count + computer2.queue.Count + computer3.queue.Count;
        meanTaskCount = meanTaskCountSum / processCount;

        if (taskCount < settings.maxTasks)
        {
            time += progress;
            if (taskInterval <= time + 0.001)
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
            var min = Utils.min(taskInterval, computer1.currentProcessingTime, computer2.currentProcessingTime, computer3.currentProcessingTime);
            Process(min);
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

    public string ToStringGeneralSett()
    {
        string str = "";

        str += "Временные параметры каждой ЭВМ: \n";
        str += computer1.GetTempStats() + "\n";
        str += computer2.GetTempStats() + "\n";
        str += computer3.GetTempStats() + "\n";

        return str;
    }

    public double AllGetTime1()
    {
       return computer1.GetTimeComp();
    }

    public double AllGetTime2()
    {
        return computer2.GetTimeComp();
    }

    public double AllGetTime3()
    {
        return computer3.GetTimeComp();
    }

    public string ToStringAVM()
    {
        string str = "";
        str += "Итоговые результаты по всем ЭВМ: \n";
        str += computer1.GetStats();
        str += computer2.GetStats();
        str += computer3.GetStats();

        return str;
    }

    public string CheckEnding(int count)
    {
        string taskWord;
        if (count % 10 == 1 && count % 100 != 11)
        {
            taskWord = "задание.";
        }
        else if ((count % 10 >= 2 && count % 10 <= 4) && !(count % 100 >= 12 && count % 100 <= 14))
        {
            taskWord = "задания.";
        }
        else
        {
            taskWord = "заданий.";
        }

        return taskWord;
    }

    public string ToStringSys()
    {
        string str = "";
        str += "Система:\n\n";
        str += "Время работы: " + Math.Round(workTime, 2) + " мин. \n\n";
        str += "Время простоя: " + Math.Round(deadTime, 2) + " мин. \n";
        double meanWorkTime = (computer1.workTime + computer2.workTime + computer3.workTime) / (computer1.completedTaskCount + computer2.completedTaskCount + computer3.completedTaskCount);
        str += "Среднее время обработки задания - " + (meanWorkTime > 0 && !double.IsInfinity(meanWorkTime) ? Math.Round(meanWorkTime, 2) : 0) + " мин. \n";
        str += "Среднее время выполнения задания - " + Math.Round(meanTimeToComplete, 2) + " мин. \n";

        //str += "Среднее время присутствия задания в системе - " + Math.Round(meanPresenceTime, 2) + " мин. \n";

        str += "Среднее число заданий в системе - " + Math.Round(meanTaskCount, 0) + " " + CheckEnding(Convert.ToInt32(meanTaskCount)) + "\n";

        //str += "Среднее количество загруженных каналов системы - " + Math.Round(meanChannelLoad, 2) + "\n";

        str += "Количество выполненных заданий: " + completedTaskCount + " " + CheckEnding(completedTaskCount) + "\n";

        return str;
    }

    public double GetWorkTime()
    {
        return Math.Round(workTime, 2);
    }
}