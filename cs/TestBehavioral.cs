using System;
using System.Collections.Generic;
using Behavioral.Strategy;
using Behavioral.State;
using Behavioral.PaymentStrategy;
using Behavioral.ChainOfResponsibility;
using Behavioral.Memento;
using Behavioral.Visistor;
using Behavioral.PersonVisitor;
using Behavioral.CompositeVisitor;
using Behavioral.Observer;
using Behavioral.Mediator;
using Behavioral.Iterator;
using Behavioral.Template;
using Behavioral.Command;
using Behavioral.Interpretator;

namespace Test
{
    class BehavioralPatterns
    {
        public static void TestMediator()
        {
            Component1 component1 = new Component1();
            Component2 component2 = new Component2();
            new ConcreteMediator(component1, component2);

            Console.WriteLine("Client triggets operation A.");
            component1.DoA();
            Console.WriteLine("Client triggets operation B.");
            component1.DoB();

            Console.WriteLine();

            Console.WriteLine("Client triggers operation C.");
            component2.DoC();
            Console.WriteLine("Client triggets operation D.");
            component2.DoD();
        }
        public static void TestObserverEvent()
        {
            EventSubject subject = new EventSubject();
            subject.CahngeStateEvent += EventHandlers.Log;
            subject.CahngeStateEvent += EventHandlers.LogEven;
            var Counter = new CounterEventObserver(state => state < 5);
            Counter.Subscibe(subject);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
            Console.WriteLine("Detach even observer");
            subject.CahngeStateEvent -= EventHandlers.LogEven;
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
        }
        public static void TestObserverDefault()
        {
            DefaultSubject subject = new DefaultSubject();
            var Logger = new ConsoleLogDefaultObserver();
            subject.Subscribe(Logger);
            var Even = new EvenDefaultObserver();
            Even.Subscribe(subject);
            var Counter = new CounterDefaultObserver(x => x < 5);
            Counter.Subscribe(subject);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
            Console.WriteLine("Detach even observer");
            subject.Unsuscribe(Even);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
        }

        public static void TestObserver()
        {
            Subject subject = new Subject();
            ICusomObserver Logger = new ConsoleLogObserver();
            ICusomObserver Even = new EvenObserver();
            ICusomObserver Counter = new CounterObserver(state => state < 5);
            subject.Attach(Logger);
            subject.Attach(Even);
            subject.Attach(Counter);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
            Console.WriteLine("Detach even observer");
            subject.Detach(Even);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
        }
        public static void TestPersonVisitor()
        {
            Student student = new Student("I??????", "I??????????????", 2);
            Printer printer = new Printer();
            Hi hi = new Hi();
            student.Accept(printer);
            student.Accept(hi);

            Professor professor = new Professor("????????????", "??????????", "??????????????????????", "??i?????????????????? i ???????????????????? ????????????????????");

            professor.Accept(printer);
            professor.Accept(hi);
        }
        public static void TestVisitor()
        {
            List<IComponent> components = new List<IComponent>
            {
                new ConcreteComponentA(),
                new ConcreteComponentB(),
                new ConcreteComponentA()
            };

            Console.WriteLine("The client code works with all visitors via the base Visitor interface:");
            var visitor1 = new ConcreteVisitor1();
            Client.ClientCode(components, visitor1);

            Console.WriteLine();

            Console.WriteLine("It allows the same client code to work with different types of visitors:");
            var visitor2 = new ConcreteVisitor2();
            Client.ClientCode(components, visitor2);
        }
        public static void TestMemento()
        {
            Originator originator = new Originator("Init state");
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            Console.WriteLine();
            caretaker.ShowHistory();

            Console.WriteLine("\nClient: Now, let's rollback!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine();
        }
        public static void TestChainOfResponsibility()
        {
            var ??hain = new LogHendler();
            ??hain
                .SetNext(new AuthorizeHendler())
                // .SetNext(new IncHendler())
                .SetNext(new LogHendler())
                .SetNext(new ResponceHendler());
            Console.WriteLine(??hain.Handle(new Request("Noname", "")));
            Console.WriteLine(??hain.Handle(new Request("admin", "admin")));
        }

        public static void TestPipline()
        {
            PiplineManager pipline = new PiplineManager(8);
            pipline.SetHandler(0, new PiplineLogHendler());
            pipline.SetHandler(2, new PiplineAuthorizeHendler());
            pipline.SetHandler(4, new PiplineLogHendler());
            pipline.SetHandler(6, new PiplineResponceHendler());

            pipline.SetHandler(3, new PiplineIncHendler());
            Console.WriteLine(pipline.Handle(new Request("Noname", "")));
            Console.WriteLine(pipline.Handle(new Request("admin", "admin")));
        }

        public static void TestState()
        {
            List<SubjectMark> Subjects = new List<SubjectMark>(){
                new SubjectMark("Unity", 95),
                new SubjectMark("Algebra", 42),
                new SubjectMark("History", 0)
            };
            Subjects.ForEach(subject => subject.Pass());
            Subjects[2].Rating = 50;
            Subjects[2].Pass();
        }
        public static void TestPaymentStrategy()
        {
            List<Card> Cards = new List<Card> {
                new Visa("1234 5678 9012 3456", new DateTime(2023,10,1), -1000),
                new MasterCard("2234 5678 9012 3477", new DateTime(2021,1,1), 5000),
                new MasterCard("3234 5678 9012 3000", new DateTime(2024,12,31), 500),
                new Visa("4234 5678 9012 3456", new DateTime(2023,10,1), 10000),
            };
            PaymentProcessor processor = new PaymentProcessor();
            processor.strategies = new Dictionary<string, IPayment>(){
                {"MASTER", new MasterCardPayment()},
                {"VISA", new VisaPayment()}
            };

            Bill bill = new Bill(600);

            foreach (var card in Cards)
            {
                if (processor.Checkout(bill, card))
                {
                    Console.WriteLine($"Payd by {card.Number}");
                    break;
                }
                else
                    Console.WriteLine($"Not payd by {card.Number}");
            }

        }
        public static void TestStrategy()
        {
            var context = new Context();

            Console.WriteLine("Client: Strategy is set to normal sorting.");
            context.SetStrategy(new ConcreteStrategyA());
            context.DoSomeBusinessLogic();

            Console.WriteLine();

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            context.SetStrategy(new ConcreteStrategyB());
            context.DoSomeBusinessLogic();

            Console.WriteLine("Client: Strategy is set to Capitalize");
            context.SetStrategy(new CapitalizeStrategy());
            context.DoSomeBusinessLogic();
        }

        public static void TestCompositVisitor()
        {
            CompositeComponent folder = new Folder("Project")
                       .Add(new MyFile("Project", "csproj"))
                       .Add(new MyFile("Program", "cs"))
                       .Add(new Folder("bin")
                           .Add(new MyFile("Program", "exe"))
                           .Add(new MyFile("config", "json"))
                       )
                       .Add(new MyFile("", "gitignore"))
                       .Add(new MyFile("README", "md"))
                       .Add(new Folder("git"))
                       as CompositeComponent;
            Console.WriteLine(folder.ToString(0));
            SortVisitor sort = new SortVisitor();
            folder.Accept(sort);
            Console.WriteLine(folder.ToString(0));
            FileCountVisitor count = new FileCountVisitor();
            folder.Accept(count);
            Console.WriteLine($"File count = {count.Count}");
        }

        public static void TestIterator()
        {
            var list = new HumanCollection(new Human[4] {
                new Human("Yurii", 32),
                new Human("Andrii", 42),
                new Human("Tetiana", 18),
                new Human("Olekandr", 62)
            });
            Human person = list.Iterator.Next();
            while (person != null)
            {
                Console.WriteLine(person);
                person = list.Iterator.Next();
            }
        }

        public static void TestStandartIterator()
        {
            var collection = new WordsCollection();
            collection.AddItem("First")
                .AddItem("Second")
                .AddItem("Third");

            Console.WriteLine("Straight traversal:");

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            Console.WriteLine("\nReverse traversal:");

            collection.ReverseDirection();

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }
        }

        public static void TestTemplate()
        {
            var f = new Equation(x => (x * x - 2) * (x + 3));
            f.bracketingMethod = new Tabulate();
            f.iterativeMethod = new BinaryDiv();
            var solves = f.Solve();
            Console.WriteLine($"Found {solves.Count} solutions");
            foreach (double root in solves)
            {
                Console.WriteLine(root);
            }

        }

        public static void TestCommand()
        {
            Invoker invoker = new Invoker();
            invoker.SetOnStart(new SimpleCommand("Say Hi!"));
            Receiver receiver = new Receiver();
            invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));
            invoker.DoSomethingImportant();
        }

        public static void TestInterpreator()
        {
            //2 + 3   2 3 +
            //a * b   a b *
            // Evaluator ex = new Evaluator ("x y z 666 + - +");
            Evaluator ex = new Evaluator("x = 20 ; x + y + 3");
            List<IExpression> v = new List<IExpression>(){
                new Variable("x", new Number(5)),
                new Variable("z", new Number(10)),
                new Variable ("y", new Number(15))
            };
            Console.WriteLine(ex.Interpret(v));
        }
    }
}