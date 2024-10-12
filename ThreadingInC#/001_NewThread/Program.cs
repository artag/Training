var t = new Thread(WriteY);
t.Start();

for (var i = 0; i < 100; i++)
    Console.Write("x");

static void WriteY()
{
    for (var i = 0; i < 100; i++)
        Console.Write("y");
}
