## 2. Извлечение данных из таблиц

Извлечение отдельных столбцов
```
SELECT prod_name
FROM Products;
```

Извлечение нескольких столбцов
```
SELECT prod_id, prod_name, prod_price
FROM Products;
```

Извлечение всех столбцов
```
SELECT *
FROM Products;
```

Извлечение уникальных строк
```
SELECT DISTINCT vend_id
FROM Products;
```

Ограничение результатов запроса
```
/* Для MSSQL и MS Access */
SELECT TOP 5 prod_name
FROM Products;
```
```
/* Для DB */
SELECT prod_name
FROM Products
FETCH FIRST 5 ROWS ONLY;
```
```
/* Для Oracle */
SELECT prod_name
FROM Products
WHERE ROWNUM <=5;
```
```
/* Для MySQL, MariaDB, PostgreSQL, SQLite */
SELECT prod_name
FROM Products
LIMIT 5;
```

```
/* Еще для MySQL, MariaDB, PostgreSQL, SQLite */
SELECT prod_name
FROM Products
LIMIT 5 OFFSET 5;
```
или
```
SELECT prod_name
FROM Products
LIMIT 3,4;
```

Использование комментариев
```
-- комментарий

# комментарий (реже используется)

/*
    Многострочный
    комментарий
*/
```


## 3. Сортировка полученных данных

Сортировка записей
```
SELECT prod_name
FROM Products
ORDER BY prod_name;
```

Сортировка по нескольким столбцам
```
SELECT prod_id, prod_price, prod_name
FROM Products
ORDER BY prod_price, prod_name;
```

Сортировка по положению (номеру) столбца
```
SELECT prod_id, prod_price, prod_name
FROM Products
ORDER BY 2, 3;
```

Указание направления сортировки (по возрастанию/по убыванию)
```
SELECT prod_id, prod_price, prod_name
FROM Products
ORDER BY prod_price DESC;
```

```
SELECT prod_id, prod_price, prod_name
FROM Products
ORDER BY prod_price DESC, prod_name;        -- prod_name ASC
```

```
SELECT prod_id, prod_price, prod_name
FROM Products
ORDER BY prod_price DESC, prod_name DESC
```


## 4. Фильтрация данных

Использование предложения WHERE
```
SELECT prod_name, prod_price
FROM Products
WHERE prod_price = 3.49;
```

Операторы в предложении WHERE

* `=` - равенство
* `<>` - неравенство
* `!=` - неравенство
* `<` - меньше
* `<=` - меньше или равно
* `!<` - не меньше
* `>` - больше
* `>=` - больше или равно
* `!>` - не больше
* `BETWEEN` - вхождение в диапазон (включая концы)
* `IS NULL` - значение NULL

Сравнение с одиночным значением
```
SELECT prod_name, prod_price
FROM Products
WHERE prod_price < 10;
```

```
SELECT prod_name, prod_price
FROM Products
WHERE prod_price <= 10;
```

```
SELECT vend_id, prod_name
FROM Products
WHERE vend_id <> 'DLL01';
```
или, тоже самое
```
SELECT vend_id, prod_name
FROM Products
WHERE vend_id != 'DLL01';
```

Сравнение с диапазоном значений
```
SELECT prod_name, prod_price
FROM Products
WHERE prod_price BETWEEN 5.99 AND 10;
```

Проверка на отсутствие значения
```
SELECT cust_name
FROM Customers
WHERE cust_email IS NULL;
```


## 5. Расщиренная фильтрация данных

Комбинирование условий WHERE. Оператор AND
```
SELECT prod_id, prod_price, prod_name
FROM Products
WHERE vend_id = 'DLL01' AND prod_price <= 4;
```

Комбинирование условий WHERE. Оператор OR
```
SELECT prod_name, prod_price
FROM Products
WHERE vend_id = 'DLL01' OR vend_id = 'BRS01';
```

Комбинирование условий WHERE. Порядок обработки операторов
```
SELECT prod_name, prod_price
FROM Products
WHERE (vend_id = 'DLL01' OR vend_id = 'BRS01')
      AND prod_price >= 10;
```

Оператор IN
```
SELECT prod_name, prod_price
FROM Products
WHERE vend_id IN ('DLL01', 'BRS01')
ORDER BY prod_name;
```
вывод похож на вывод после такого запроса:
```
SELECT prod_name, prod_price
FROM Products
WHERE vend_id = 'DLL01' OR vend_id = 'BRS01'
ORDER BY prod_name;
```

Оператор NOT
```
SELECT prod_name
FROM Products
WHERE NOT vend_id = 'DLL01'
ORDER BY prod_name;
```
аналогичен этому запросу:
```
SELECT prod_name
FROM Products
WHERE vend_id <> 'DLL01'
ORDER BY prod_name;
```
