using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpSandbox
{
    /// <summary>
    /// Counter class - track a class type, and a counter for the number of instances of the class created.
    /// This object is used in a list (listCounters) in the Parent
    /// </summary>
    public class Counter
    {
        public Type ClassType { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Parent class - Maintain a list of Counter objects which hold a class type and counter representing the number of instances a class created.
    /// This class is derived from the IDisposable interface which exposes/requires the Dispose method related to garbage collection.
    /// </summary>
    public class Parent : IDisposable
    {
        private bool disposed = false;

        private static List<Counter> listCounters = new List<Counter>();
        public static int TheCount(Type type)
        {
            Counter myClass = listCounters.Find(counterObject => counterObject.ClassType == type);
            int count = (myClass == null) ? 0 : myClass.Count;
            return count;
        }

        /// <summary>
        /// Find the ClassType in listCounters.  Add it if is not yet in the list.  Increment the count.
        /// </summary>
        /// <param name="type"></param>
        public Parent(Type type)
        {
            Counter myClass = listCounters.Find(counterObject => counterObject.ClassType == type);
            if (myClass == null)
            {
                myClass = new Counter();
                myClass.ClassType = type;
                myClass.Count++;
                listCounters.Add(myClass);
            }
            else
            {
                myClass.Count++;
            }
        }

        /// <summary>
        /// Deal with garbage collection - call Dispose
        /// </summary>
        ~Parent()
        {
            //Console.WriteLine("Parent being finalized");
            this.Dispose();
        }

        /// <summary>
        /// Deal with garbage collection
        /// </summary>
        public void Dispose()
        {
            Counter node = listCounters.Find(counterObject => counterObject.ClassType == this.GetType());
            node.Count = node.Count - 1;
            //if (!this.disposed)
            //{
            //    Console.WriteLine("Parent being disposed");
            //}
            this.disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    public class CountMe : Parent
    {
        public CountMe() : base(typeof(CountMe))
        {
        }
    }

    public class CountMeToo : Parent
    {
        public CountMeToo() : base(typeof(CountMeToo))
        {
        }
    }

    public class AndMe : Parent
    {
        public AndMe() : base(typeof(AndMe))
        {
        }
    }

    public class Counted : Parent
    {
        public Counted() : base(typeof(Counted))
        {
        }
    }

    public class ZeroInstances : Parent
    {
        public ZeroInstances() : base(typeof(ZeroInstances))
        {
        }
    }

    class Program
    {
        /// <summary>
        /// Create a set (list) of objects of class type T
        /// Print out the set name and the number of objects in each set to the console.
        /// </summary>
        /// <typeparam name="T">Generics: T - is the class type (to be counted)</typeparam>
        /// <param name="instances">number of class objects to add to the list</param>
        /// <returns>List of classes.</returns>
        static List<T> Factory<T>(int instances)
        {
            List<T> classList = new List<T>();
            for (int i = 0; i < instances; i++)
            {
                // This step is like a new - but works with generics.  Add the new node to the list.
                classList.Add((T)Activator.CreateInstance<T>());
            }
            Console.WriteLine("{0} {1}", typeof(T).Name, Parent.TheCount(typeof(T)));

            return classList;
        }

        static void Main(string[] args)
        {
            List<CountMe> countMe = Factory<CountMe>(5);
            List<CountMeToo> countMeToo = Factory<CountMeToo>(4);
            List<AndMe> andMe = Factory<AndMe>(12);
            List<Counted> counted = Factory<Counted>(5);
            List<ZeroInstances> zeros = Factory<ZeroInstances>(0);
            zeros = Factory<ZeroInstances>(-1);

            // Track count when removing a class instance prior to garbage collection
            CountMe node = countMe[0];
            countMe.Remove(node);
            node.Dispose();
            Console.WriteLine("\nAfter removing and disposing, CountMe: {0}\n", countMe.Count);
        }
    }
}

