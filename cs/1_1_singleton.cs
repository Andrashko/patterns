using System;
namespace Creational
{
    namespace Singleton
    {
        /* приклад шалону Одинак.
        Зберігає випадкове число та лічильник викликів. 
        */
        class Singleton
        {
            private static Singleton _instance = null;
            public double randomNumber;
            private int counter = 0;

            private Singleton()
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                this.randomNumber = rnd.Next();
            }

            public static Singleton getInstance()
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }
                return _instance;
            }

            public void Print()
            {
                Console.WriteLine($"My random number =  {this.randomNumber} \n Counter = {this.counter}");
            }

            public void IncCounter()
            {
                this.counter++;
            }
        }
    }
}