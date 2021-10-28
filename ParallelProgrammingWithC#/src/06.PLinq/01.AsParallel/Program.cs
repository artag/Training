using System;
using System.Linq;
using System.Threading.Tasks;

namespace AsParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            //UnorderedParallel.Execute();
            OrderedParallel.Execute();
        }
    }
}
