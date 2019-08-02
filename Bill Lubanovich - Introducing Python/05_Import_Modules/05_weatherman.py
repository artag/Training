# Импортируем модуль
import report
description = report.get_description()
print("Today's weather:", description)
print('---')


# Импортируем модуль с другим именем
import report as wr
description = wr.get_description()
print("Today's weather second impl:", description)
print('---')


# Импортируем только самое необходимое
from report import get_description
description = get_description()
print("Today's weather third impl:", description)
print('---')

# Теперь импортируем ее как do_it:
from report import get_description as do_it
description = do_it()
print("Today's weather forth impl:", description)
