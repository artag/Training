using System;

namespace DependencyInjection
{
    interface IService
    {
        void DoIt();
    }

    class ServiceOne : IService
    {
        public void DoIt()
        {
            Console.WriteLine("Service one");
        }
    }

    class ServiceTwo : IService
    {
        public void DoIt()
        {
            Console.WriteLine("Service two");
        }
    }
}
