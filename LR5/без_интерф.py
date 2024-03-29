import numpy as np
import matplotlib.pyplot as plt
from enum import Enum 

def intervalsToTimeArray(arr):
    newArr = np.zeros(len(arr))
    for i in range(len(arr)):
        for j in range(i+1):
            newArr[i]+=arr[j]
    return newArr

States = Enum('States', 'waiting setup execute repair')

# Количество деталей для обработки
n = 500

# Интервалы между заданиями, распределенные экспоненциально
intervals = np.random.exponential(scale=1.0, size=n)
intervals = intervalsToTimeArray(intervals)

# Время наладки станка, равномерно распределенное на интервале 0.2-0.5 часа
setup_times = np.random.uniform(low=0.2, high=0.5, size=n)

# Время выполнения заданий, нормально распределенное с мат. ожиданием 0.5 часа и среднеквадратичным отклонением 0.1 часа
task_times = np.random.normal(loc=0.5, scale=0.1, size=n)

# Интервалы между поломками, нормально распределенные с мат. ожиданием 20 часов и среднеквадратичным отклонением 2 часа
break_intervals = np.random.normal(loc=20, scale=2, size=n*2)
break_intervals = intervalsToTimeArray(break_intervals)

# Время устранения поломки, равномерно распределенное на интервале от 0.1 до 0.5 часа
repair_times = np.random.uniform(low=0.1, high=0.5, size=n*2)

# переменные для вывода статистики
time_of_non_work = 0
intervals_between_start_task = np.zeros(n)
execute_time = np.zeros(n)
start_task_time = np.zeros(n)

# основные рабочие переменные
current_time = 0
state = States.waiting
task_number = 0
break_number = 0
k=0
while True:
    if state == States.waiting:
        if (task_number == len(intervals)):
            # выполнение завершено
            break
        # если нужно ждать - ждём
        if (current_time < intervals[task_number]):
            time_of_non_work += intervals[task_number] - current_time
            current_time =  intervals[task_number]
        
        start_task_time[task_number] = current_time
        intervals_between_start_task[task_number] = current_time - intervals_between_start_task[task_number-1]

        state = States.setup
    elif state == States.setup:
        # если станок сломается на этапе подготовки
        if (current_time + setup_times[task_number] > break_intervals[break_number]):
            current_time = break_intervals[break_number]
            state = States.repair
            k+=1
        else:
            current_time += setup_times[task_number]
            state = States.execute
    elif state == States.execute:
        # если станок сломается на этапе выполнения
        if (current_time + task_times[task_number] > break_intervals[break_number]):
            current_time = break_intervals[break_number]
            state = States.repair
            k+=1
        else:
            current_time += task_times[task_number]

            execute_time[task_number] = current_time - start_task_time[task_number]

            task_number+=1
            state = States.waiting
    elif state == States.repair:
        time_of_non_work += repair_times[break_number]
        current_time += repair_times[break_number]
        break_number+=1
        state = States.setup

print(f"Общее время работы станка: {current_time:.2f} ч.")
print(f"Времени простоя станка: {time_of_non_work:.2f} ч.")
print(f"Среднее время между заданиями: {np.mean(intervals_between_start_task[1:]):.2f} ч.")
print(f"Среднее время выполнения задания: {np.mean(execute_time):.2f} ч.")
print(f"Максимальное между заданиями: {(max(intervals_between_start_task[1:])):.2f} ч.")
print(f"Количество поломок: {k}")

fig, ax = plt.subplots(figsize=(10, 5))
ax.plot(range(n), intervals, label='Время между заданиями')
ax.set_xlabel('Номер задания')
ax.set_ylabel('Время')

ax.legend()
plt.show()