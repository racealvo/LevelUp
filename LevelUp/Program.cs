using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp
{
    /// <summary>
    /// This is the workhorse.  It contains the class counter.
    /// Encapsulate counter - to be safe.  This will require instance access rather than class access
    /// </summary>
    /// <typeparam name="T">Generics: T - is the class type (to be counted)</typeparam>
    public class Parent<T> 
    {
        private static int counter;

        public Parent()
        {
            counter++;
        }

        public int Counter { get { return counter; } }
    }

    public class CountMe
    {
    }

    public class CountMeToo
    {
    }

    public class AndMe
    {
    }

    public class Counted
    {
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
        static List<Parent<T>> Factory<T>(int instances)
        {
            List<Parent<T>> classList = new List<Parent<T>>();
            for (int i = 0; i < instances; i++)
            {
                classList.Add(new Parent<T>());
            }
            Console.WriteLine("{0} {1}", typeof(T).Name, classList[0].Counter);

            return classList;
        }

        /// <summary>
        /// This is a little contrived.  Since everything is being stored in lists, 
        /// we could simply count the members in each list.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            List<Parent<CountMe>> countMe       = Factory<CountMe>(5);
            List<Parent<CountMeToo>> countMeToo = Factory<CountMeToo>(4);
            List<Parent<AndMe>> andMe           = Factory<AndMe>(12);
            List<Parent<Counted>> counted       = Factory<Counted>(5);
        }
    }
}
