using System;
namespace Creational
{
    namespace Singleton
    {
        class Singleton
        {
            private static Singleton instance = null;

            private Singleton()
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                this.randomNumber = rnd.Next();
            }

            public static Singleton getInstance()
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }

            public double randomNumber;
            private int counter = 0;

            public void Print()
            {
                Console.WriteLine($"My random number =  {this.randomNumber}");
            }

            public void IncCounter()
            {
                Console.WriteLine($"Counter = {++this.counter}");
            }
        }
    }
}