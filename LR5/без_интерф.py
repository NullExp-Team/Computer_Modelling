import numpy as np
import matplotlib.pyplot as plt

# Количество деталей для обработки и массивы для хранения времени начала и окончания каждого задания и поломки
n = 500
start_times = np.zeros(n)
end_times = np.zeros(n)
break_end_times = np.zeros(n)
current_time = 0
total_time = 0
completion_times = []
break_count = 0

# Интервалы между заданиями, распределенные экспоненциально
# Время наладки станка, равномерно распределенное на интервале 0.2-0.5 часа
# Время выполнения заданий, нормально распределенное с мат. ожиданием 0.5 часа и среднеквадратичным отклонением 0.1 часа
# Интервалы между поломками, нормально распределенные с мат. ожиданием 20 часов и среднеквадратичным отклонением 2 часа
# Время устранения поломки, равномерно распределенное на интервале от 0.1 до 0.5 часа
intervals = np.random.exponential(scale=1.0, size=n)
setup_times = np.random.uniform(low=0.2, high=0.5, size=n)
task_times = np.random.normal(loc=0.5, scale=0.1, size=n)
break_intervals = np.random.normal(loc=20, scale=2, size=n)
repair_times = np.random.uniform(low=0.1, high=0.5, size=n)

for i in range(n):
    # Рассчитываем время начала и окончания
    start_times[i] = current_time + setup_times[i] + max(0, end_times[i-1] - current_time) if i > 0 else current_time + setup_times[i]
    end_times[i] = start_times[i] + task_times[i]
    next_task_time = start_times[i] + intervals[i]
    if next_task_time >= break_intervals[i-1]:
    # Если поломки нет, то настраиваем станок и выполняем задание
        next_task_time += setup_times[i] + task_times[i]
        total_time += setup_times[i] + task_times[i]
        completion_times.append(end_times[i])
        break_end_times = np.insert(break_end_times, i, break_end_times[i-1] + np.random.uniform(0.1, 0.5))
    else:
    # Если поломка произошла, то перемещаем текущее задание в начало очереди
        break_count+=1
        start_times = np.insert(start_times, i, start_times[i])
        end_times = np.insert(end_times, i, end_times[i])
        intervals = np.insert(intervals, i, intervals[i])
        task_times = np.insert(task_times, i, task_times[i])
        break_end_times = np.insert(break_end_times, i, break_end_times[i-1] + np.random.uniform(0.1, 0.5))
        start_times = np.delete(start_times, i+1)
        end_times = np.delete(end_times, i+1)
        intervals = np.delete(intervals, i+1)
        task_times = np.delete(task_times, i+1)
        break_end_times = np.delete(break_end_times, i+1)
        i -= 1

exit_string = ''
exit_string += f"Общее время работы станка: {total_time:.2f} ч." + '            '
exit_string += f"Времени простоя станка: {total_time*(1 - total_time/500):.2f} ч." + '\n'
exit_string += f"Среднее время между заданиями: {np.mean(intervals):.2f} ч." + '            '
exit_string += f"Среднее время выполнения задания: {np.mean(task_times):.2f} ч." + '\n'
exit_string += f"Максимальное время ожидания задания: {(max(np.array(completion_times) - np.array(start_times))):.2f} ч." + '            '
exit_string += f"Количество поломок станка: {break_count} "

fig, ax = plt.subplots(figsize=(13, 5))
ax.text(-100, max(np.array(intervals))*1.06, exit_string)
ax.plot(range(n), intervals, label='Время между заданиями', color=(0,0.6,0),marker = '.')
ax.set_facecolor((1,1,1))
fig.set_facecolor((0.8,0.8,0.8))
fig.set_edgecolor((0,1,0))
ax.set_xlabel('№ задания')
ax.set_ylabel('Время выполнения')



ax.legend()
plt.show()