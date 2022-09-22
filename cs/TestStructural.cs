using System;
using Structural.Proxy;
using Structural.Decorator;
using Structural.Game;
using Structural.Adapter;
using Structural.Facade;
using Structural.Bridge;
using Structural.Composite;
using Structural.Flyweight;

using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Test
{
    class StructuralPaterns
    {


        public static void TestFlyweight()
        {
            /* При ініціалізації додатку можливе заповннення деяких спільних станів.
            */

            var factory = new FlyweightFactory(
                new Car { Company = "Chevrolet", Model = "Camaro" },
                new Car { Company = "Mercedes Benz", Model = "C300" },
                new Car { Company = "BMW", Model = "M5" },
                new Car { Company = "BMW", Model = "X6" }
            );

            factory.listFlyweights();

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "Jhone Snow",
                Company = "BMW",
                Model = "M5",
                Color = "red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",
                Color = "red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CR123IR",
                Owner = "Jhone Doe",
                Company = "BMW",
                Model = "X1",
                Color = "black"
            });

            factory.listFlyweights();

            Console.WriteLine("List of cars:");
            foreach (var car in Cars)
            {
                Console.WriteLine(car.GetStandartObject().ToString());
            }
        }

        private static List<Flyweight> Cars = new List<Flyweight>();

        public static void addCarToPoliceDatabase(FlyweightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");
            Flyweight flyweightCar = factory.GetFlyweight(car);
            Cars.Add(flyweightCar);
            flyweightCar.Print();
        }

        public static void TestBridge()
        {
            Abstraction abstractionA = new Abstraction(new ConcreteImplementationA());
            Abstraction abstractionB = new Abstraction(new ConcreteImplementationB());
            Abstraction exAbstractionA = new ExtendedAbstraction(new ConcreteImplementationA());
            Abstraction exAbstractionB = new ExtendedAbstraction(new ConcreteImplementationB());
            Console.WriteLine(abstractionA.Operation());
            Console.WriteLine(abstractionB.Operation());
            Console.WriteLine(exAbstractionA.Operation());
            Console.WriteLine(exAbstractionB.Operation());
        }
        public static void TestProxy()
        {
            ISubject subject = new RealSubject("8.8.8.8");
            Console.WriteLine(subject.Request());
            subject = new Proxy(subject as RealSubject);
            Console.WriteLine(subject.Request());
            Console.WriteLine(subject.Request());
            subject = new Proxy("127.0.0.1");
            Console.WriteLine(subject.Request());
            Console.WriteLine(subject.Request());
        }

        public static void TestDecorator()
        {
            IComponent component = new ConcreteComponent();
            Console.WriteLine(component.Operation());
            Decorator decorator = new ConcreteDecoratorA(component);
            Console.WriteLine(decorator.Operation());
            Console.WriteLine((decorator as ConcreteDecoratorA).OtherOperation().ToString());
            decorator = new ConcreteDecoratorB(decorator);
            Console.WriteLine(decorator.Operation());
        }

        public static void TestGame()
        {
            IDamageActor humen = new Character("Humen", 300, 50);
            IDamageActor orc = new Character("Orc", 350, 75);
            humen = new DefenceBuff(new DefenceBuff(humen, 10), 50);

            while (!humen.IsDead() && !orc.IsDead())
            {
                humen.Hit(orc);
                orc.Hit(humen);
            }
        }

        public static void TestAdapter()
        {
            Adaptee oldLib = new Adaptee();
            Console.WriteLine(oldLib.GetSpecificRequest("Test", 4, true));
            Adapter newLib = new Adapter(oldLib);
            Console.WriteLine(newLib.GetRequest("Test"));
        }

        public static void TestAdapterWinApi()
        {
            Process process = new Process();
            ProcessInformation pi = new ProcessInformation();
            Startupinfo si = new Startupinfo();
            si.cb = Marshal.SizeOf(si);
            process.Create(null, "notepad.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi);
            AdaptedPocess adaptedPocess = new AdaptedPocess(process);
            adaptedPocess.Create("notepad.exe");
        }


        public static void TestFacade()
        {
            Console.WriteLine(ArtFacade.Get("Music", 1));
            Console.WriteLine(ArtFacade.Get("Movie", 1));
            Console.WriteLine(ArtFacade.Get("TVShow", 1));
            Console.WriteLine(ArtFacade.Get("Book", 2));
        }

        public static void TestComposite()
        {
            MyFile file = new MyFile("new", "cs");
            CompositeComponent folder = new Folder("Project")
            .Add(new MyFile("Project", "csproj"))
            .Add(new MyFile("Program", "cs"))
            .Add(new Folder("bin")
                .Add(new MyFile("Program", "exe"))
                .Add(new MyFile("config", "json"))
                .Add(file)
            )
            .Add(new MyFile("", "gitignore"))
            .Add(new MyFile("README", "md"))
            .Add(new Folder("git"))
            as CompositeComponent;
            Console.WriteLine(folder.ToString(0));
            folder.Remove(file);
            Console.WriteLine(folder.ToString(0));
            folder.Sort();
            Console.WriteLine(folder.ToString(0));
        }
    }
}