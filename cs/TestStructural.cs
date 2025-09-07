using System;
using Structural.Proxy;
using Structural.Decorator;
using Structural.Game;
using Structural.Adapter;
using Structural.Facade;
using Structural.Bridge;
using Structural.Composite;
using Structural.Flyweight;
using Structural.Monads;

using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Test
{
    class StructuralPatterns
    {
        public static void TestFlyweight()
        {
            /* При ініціалізації додатку можливе заповннення деяких спільних станів.
            */

            FlyweightFactory factory = new FlyweightFactory(
                new Car { Company = "Chevrolet", Model = "Camaro" },
                new Car { Company = "Mercedes Benz", Model = "C300" },
                new Car { Company = "BMW", Model = "M5" },
                new Car { Company = "BMW", Model = "X6" }
            );

            factory.listSharedStates();

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "Jon Snow",
                Company = "BMW",
                Model = "M5",
                Color = "Red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "Skoda",
                Model = "Octavia",
                Color = "Red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CR123IR",
                Owner = "Jon Doe",
                Company = "Skoda",
                Model = "Octavia",
                Color = "Black"
            });

            factory.listSharedStates();

            Console.WriteLine("List of cars:");
            foreach (var car in Cars)
            {
                Console.WriteLine(car.GetStandartObject());
            }
        }

        private static List<Flyweight> Cars = new List<Flyweight>();

        private static void addCarToPoliceDatabase(FlyweightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");
            Flyweight flyweightCar = factory.GetFlyweight(car);
            Cars.Add(flyweightCar);
            Console.WriteLine(flyweightCar);
        }

        public static void TestMemory()
        {
            int count = 2_000_000;
            long memoryUsed;
            List<Car> Cars = new List<Car>();
            FlyweightFactory factory = new FlyweightFactory();
            List<Flyweight> CarFlyweights = new List<Flyweight>();

            for (int i = 0; i < count; i++)
            {
                Car car = new Car()
                {
                    Number = $"AB{i}CD",
                    Owner = "Jon Doe",
                    Company = "Skoda",
                    Model = "Fabia",
                    Color = "Black"
                };
                Cars.Add(car);
            }

            Console.WriteLine($"Created {count} objects {Cars[count - 1]}");
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory used {memoryUsed / 1024 / 1024} Mb");
            Cars.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            for (int i = 0; i < count; i++)
            {
                Flyweight car = factory.GetFlyweight(new Car
                {
                    Number = $"AB{i}CD",
                    Owner = "Jon Doe",
                    Company = "Skoda",
                    Model = "Fabia",
                    Color = "Black"
                });
                CarFlyweights.Add(car);
            }
            Console.WriteLine($"Created {count} flyweights {CarFlyweights[count - 1]}");
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory used {memoryUsed / 1024 / 1024} Mb");
            CarFlyweights.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            for (int i = 0; i < count; i++)
            {
                Car car = new Car()
                {
                    Number = $"AB{i}CD",
                    Owner = "Jon Doe",
                    Company = new String('c', 255),
                    Model = new String('m', 255),
                    Color = "Black"
                };
                Cars.Add(car);
            }

            Console.WriteLine($"Created {count} objects {Cars[count - 1]}");
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory used {memoryUsed / 1024 / 1024} Mb");
            Cars.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();


            for (int i = 0; i < count; i++)
            {
                Flyweight car = factory.GetFlyweight(new Car
                {
                    Number = $"AB{i}CD",
                    Owner = "Jon Doe",
                    Company = new String('c', 255),
                    Model = new String('m', 255),
                    Color = "Black"
                });
                CarFlyweights.Add(car);
            }
            Console.WriteLine($"Created {count} flyweights {CarFlyweights[count - 1]}");
            memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory used {memoryUsed / 1024 / 1024} Mb");
            CarFlyweights.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            subject = new Proxy(subject);
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
            IDamageActor human = new Character("Human", 300, 50);
            IDamageActor orc = new Character("Orc", 350, 75);
            human = new DefenceBuff(new DefenceBuff(human, 10), 50);
            int count = 3;
            while (!human.IsDead() && !orc.IsDead())
            {
                human.Hit(orc);
                orc.Hit(human);
                if (count-- == 0)
                {
                    Console.WriteLine("50% Buff ends");
                    human = (human as CharacterBuff).Undecorate();
                }
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
            StartupInfo si = new StartupInfo();
            si.cb = Marshal.SizeOf(si);
            process.Create(null, "notepad.exe", IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi);
            AdaptedProcess adaptedProcess = new AdaptedProcess(process);
            adaptedProcess.Create("notepad.exe");
        }


        public static void TestFacade()
        {
            Console.WriteLine(ArtFacade.Get("Music", 1));
            Console.WriteLine(ArtFacade.Get("Movie", 1));
            Console.WriteLine(ArtFacade.Get("TVShow", 1));
            Console.WriteLine(ArtFacade.Get("Book", 1));
        }

        public static void TestComposite()
        {
            MyFile file = new MyFile("new", "cs");
            CompositeComponent folder = new Folder("Project");
            folder.Add(new MyFile("Project", "csproj"))
            .Add(new MyFile("Program", "cs"))
            .Add(new Folder("bin")
                .Add(new MyFile("Program", "exe"))
                .Add(new MyFile("config", "json"))
                .Add(file)
            )
            .Add(new MyFile("", "gitignore"))
            .Add(new MyFile("README", "md"))
            .Add(new Folder("git"))
            .Add(file);
            Console.WriteLine(folder.ToString(0));
            folder.Remove(file);
            Console.WriteLine($"File {file} was removed");
            Console.WriteLine(folder.ToString(0));
            folder.Sort();
            Console.WriteLine("order by name");
            Console.WriteLine(folder.ToString(0));
        }

        public static void TestMonad()
        {


            var repo = new TraditionalRepository();
            var monadicRepo = new MonadicRepository();
            Console.WriteLine("Find existing shipper");
            int id = 1;
            var shipper = GetShipperOfLastOrderOnCurrentAddress(repo, id);
            Console.WriteLine(shipper);
            shipper = GetShipperOfLastOrderOnCurrentAddress(monadicRepo, id);
            Console.WriteLine(shipper);
            Console.WriteLine("Find not existing shipper");
            id = -1;
            shipper = GetShipperOfLastOrderOnCurrentAddress(repo, id);
            Console.WriteLine(shipper);
            shipper = GetShipperOfLastOrderOnCurrentAddress(monadicRepo, id);
            Console.WriteLine(shipper);
        }

        private static Shipper GetShipperOfLastOrderOnCurrentAddress(ITraditionalRepository repo, int customerId)
        {
            Shipper shipperOfLastOrderOnCurrentAddress = null;
            var customer = repo.GetCustomer(customerId);
            if (customer?.address != null)
            {
                var address = repo.GetAddress(customer.address.id);
                if (address?.lastOrder != null)
                {
                    var order = repo.GetOrder(address.lastOrder.id);
                    shipperOfLastOrderOnCurrentAddress = order?.shipper;
                }
            }
            return shipperOfLastOrderOnCurrentAddress;
        }

        private static Shipper GetShipperOfLastOrderOnCurrentAddress(IMonadicRepository repo, int customerId)
        {
            Shipper shipperOfLastOrderOnCurrentAddress =
                repo.GetCustomer(customerId)
                .Bind(customer => customer.address)
                .Bind(address => repo.GetAddress(address.id))
                .Bind(address => address.lastOrder)
                .Bind(lastOrder => repo.GetOrder(lastOrder.id))
                .Bind(order => order.shipper)
                .GetValue();
            return shipperOfLastOrderOnCurrentAddress;
        }

    }

}