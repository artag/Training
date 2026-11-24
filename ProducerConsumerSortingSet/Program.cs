using System.Collections;
using System.Collections.Concurrent;

namespace ProducerConsumer;

public class Program
{
    public static async Task Main()
    {
        var data = new List<Dto>();
        for (var i  = 0; i < 100; i++)
        {
            var t = i;
            var d = new Dto() { Date = DateTime.Now.AddDays(-t), Uid = t };
            data.Add(d);
        }

        data.Shuffle();

        var q = new PCQueue(new DtoComparer());
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += new ConsoleCancelEventHandler((e, c) =>
        {
            cts.Cancel();
        });

        while (!cts.IsCancellationRequested)
        {
            foreach (var dto in data)
            {
                q.Collection.Add(dto);
            }
        }

        foreach (var dto in data)
        {
            q.Add(dto);
            Console.WriteLine(dto);
        }

        Console.WriteLine();
        Consume(q.Collection);
    }

    private static void Consume(BlockingCollection<Dto> taskQ)
    {
        foreach (var d in taskQ.GetConsumingEnumerable())
        {
            Console.WriteLine(d);
        }
    }
}

public static class ListExtensions
{
    private static Random rnd = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            var k = rnd.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}

public record Dto
{
    public DateTime Date { get; init; }

    public int Uid { get; init; }

    public override string ToString()
    {
        return Uid.ToString();
    }
}

public class DtoComparer : IComparer<Dto>
{
    public int Compare(Dto y, Dto x)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Date.CompareTo(y.Date);
    }
}

public class PCQueue : IDisposable
{
    readonly BlockingCollection<Dto> _taskQ;


    public PCQueue(IComparer<Dto> comparer)
    {
        _taskQ = new BlockingCollection<Dto>(new ConcurrentSet<Dto>(comparer), 100);
    }

    public BlockingCollection<Dto> Collection => _taskQ;

    public void Dispose()
    {
        _taskQ.CompleteAdding();
        _taskQ.Dispose();
    }

    public void Add(Dto dto)
    {
        _taskQ.Add(dto);
    }
}

public class ConcurrentSet<T> : IProducerConsumerCollection<T>
{
    private readonly object _lock = new object();
    private readonly SortedSet<T> _set;

    public ConcurrentSet(IComparer<T> comparer)
    {
        _set = new SortedSet<T>(comparer);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _set.GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    public int Count => _set.Count();
    public bool IsSynchronized => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();

    public void CopyTo(T[] array, int index)
    {
        throw new NotImplementedException();
    }

    public T[] ToArray()
    {
        throw new NotImplementedException();
    }

    public bool TryAdd(T item)
    {
        lock (_lock)
        {
            _set.Add(item);
            return true;
        }
    }

    public bool TryTake(out T item)
    {
        lock (_lock)
        {
            var i = _set.FirstOrDefault();
            if (i == null)
            {
                item = default(T);
                return false;
            }

            _set.Remove(i);
            item = i;
            return true;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}