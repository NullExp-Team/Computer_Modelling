import numpy as np
import matplotlib.pyplot as plt

# Количество деталей для обработки
n = 500

# Интервалы между заданиями, распределенные экспоненциально
intervals = np.random.exponential(scale=1.0, size=n)

# Время наладки станка, равномерно распределенное на интервале 0.2-0.5 часа
setup_times = np.random.uniform(low=0.2, high=0.5, size=n)

# Время выполнения заданий, нормально распределенное с мат. ожиданием 0.5 часа и среднеквадратичным отклонением 0.1 часа
task_times = np.random.normal(loc=0.5, scale=0.1, size=n)

# Интервалы между поломками, нормально распределенные с мат. ожиданием 20 часов и среднеквадратичным отклонением 2 часа
break_intervals = np.random.normal(loc=20, scale=2, size=n)

# Время устранения поломки, равномерно распределенное на интервале от 0.1 до 0.5 часа
repair_times = np.random.uniform(low=0.1, high=0.5, size=n)

# Массивы для хранения времени начала и окончания каждого задания и поломки
start_times = np.zeros(n)
end_times = np.zeros(n)
break_start_times = np.zeros(n)
break_end_times = np.zeros(n)

# Начальное время равно 0
current_time = 0
total_time = 0
completion_times = []

for i in range(n):
    # Рассчитываем время начала задания, учитывая наладку станка и время ожидания, если задание уже началось, но станок еще занят
    start_times[i] = current_time + setup_times[i] + max(0, end_times[i-1] - current_time) if i > 0 else current_time + setup_times[i]
    # Рассчитываем время окончания задания
    end_times[i] = start_times[i] + task_times[i]
    # Рассчитываем время следующего задания
    next_task_time = start_times[i] + intervals[i]
    # Проверяем, произошла ли поломка перед выполнением задания
    if next_task_time < break_end_times[i-1]:
    # Если поломка произошла, то перемещаем текущее задание в начало очереди
        start_times = np.insert(start_times, i, start_times[i])
        end_times = np.insert(end_times, i, end_times[i])
        intervals = np.insert(intervals, i, intervals[i])
        task_times = np.insert(task_times, i, task_times[i])
        break_end_times = np.insert(break_end_times, i, break_end_times[i-1] + np.random.uniform(0.1, 0.5))
        # Задание перемещено в начало, удаляем его из текущей позиции
        start_times = np.delete(start_times, i+1)
        end_times = np.delete(end_times, i+1)
        intervals = np.delete(intervals, i+1)
        task_times = np.delete(task_times, i+1)
        break_end_times = np.delete(break_end_times, i+1)
        # Для следующей итерации i смещается на 1 назад, чтобы повторно не проверять перемещенное задание
        i -= 1
    else:
        # Если поломки нет, то настраиваем станок и выполняем задание
        setup_time = np.random.uniform(0.2, 0.5)
        task_time = np.random.normal(0.5, 0.1)
        # Добавляем время наладки и выполнения задания к времени следующего задания
        next_task_time += setup_times[i] + task_times[i]
        # Добавляем время выполнения задания к общему времени работы станка
        total_time += setup_times[i] + task_times[i]
        # Записываем время окончания выполнения задания
        completion_times.append(end_times[i])
        break_end_times = np.insert(break_end_times, i, break_end_times[i-1] + np.random.uniform(0.1, 0.5))

print(f"Общее время работы станка: {total_time:.2f} ч.")
print(f"Процент времени простоя станка: {(1 - total_time/500)*100:.2f}%")
print(f"Среднее время между заданиями: {np.mean(intervals):.2f} ч.")
print(f"Среднее время выполнения задания: {np.mean(task_times):.2f} ч.")
k=round((max(np.array(completion_times) - np.array(start_times))),2)
print(f"Максимальное время ожидания задания: {k} ч.")

fig, ax = plt.subplots(figsize=(10, 5))
ax.plot(range(n), start_times, label='Время начала')
ax.plot(range(n), end_times, label='Время окончания')
ax.set_xlabel('Номер задания')
ax.set_ylabel('Время')
ax.set_title('Зависимость времени начала и окончания заданий от их номера')
ax.legend()
plt.show()