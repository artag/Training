minutes = 60
seconds = 60
hours = 24

# Сколько секунд в часе?
seconds_per_hour = minutes * seconds
print('Seconds per hour:', seconds_per_hour)

# Сколько секунд в сутках?
seconds_per_day = hours * seconds_per_hour
print('Seconds per day:', seconds_per_day)

# Проверка
print('seconds_per_day / seconds_per_hour =', seconds_per_day / seconds_per_hour)
print('seconds_per_day // seconds_per_hour =', seconds_per_day // seconds_per_hour)
