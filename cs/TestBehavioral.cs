using System;
using System.Collections.Generic;
using Behavioral.Strategy;
using Behavioral.State;
using Behavioral.PaymentStrategy;
using Behavioral.ChainOfResponsibility;
using Behavioral.Memento;
using Behavioral.Visitor;
using Behavioral.PersonVisitor;
using Behavioral.CompositeVisitor;
using Behavioral.Observer;
using Behavioral.Mediator;
using Behavioral.Iterator;
using Behavioral.Template;
using Behavioral.Command;
using Behavioral.Interpreter;


namespace Test
{
    class BehavioralPatterns
    {
        public static void TestPhone()
        {
            var phone = new Phone();
            phone.DialNumber("911");
            phone.PressButton();
            phone.DialNumber("911");
            phone.PressButton();
        }
        public static void TestMediator()
        {
            Component1 component1 = new Component1();
            Component2 component2 = new Component2();
            new ConcreteMediator(component1, component2);

            Console.WriteLine("Client triggers operation A.");
            component1.DoA();
            Console.WriteLine("Client triggers operation B.");
            component1.DoB();

            Console.WriteLine();

            Console.WriteLine("Client triggers operation C.");
            component2.DoC();
            Console.WriteLine("Client triggers operation D.");
            component2.DoD();
        }
        public static void TestObserverEvent()
        {
            EventSubject subject = new EventSubject();
            subject.ChangeStateEvent += EventHandlers.Log;
            subject.ChangeStateEvent += EventHandlers.LogEven;
            var Counter = new CounterEventObserver(state => state < 5);
            Counter.Subscribe(subject);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
            Console.WriteLine("Detach even observer");
            subject.ChangeStateEvent -= EventHandlers.LogEven;
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
            subject.Unsubscribe(Even);
            for (int i = 0; i < 5; i++)
                subject.GenerateRandomState();
        }

        public static void TestObserver()
        {
            Subject subject = new Subject();
            ICustomObserver Logger = new ConsoleLogObserver();
            ICustomObserver Even = new EvenObserver();
            ICustomObserver Counter = new CounterObserver(state => state < 5);
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
            Student student = new Student("Iван", "Iваненко", 2);
            Printer printer = new Printer();
            Hi hi = new Hi();
            student.Accept(printer);
            student.Accept(hi);

            Professor professor = new Professor("Микола", "Маляр", "Миколайович", "Кiбернетики i прикладної математики");

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
            var chain = new LogHandler();
            chain
                .SetNext(new AuthorizeHandler())
                .SetNext(new RoleHandler())
                .SetNext(new IncHandler())
                .SetNext(new LogHandler())
                .SetNext(new ResponseHandler());
            Console.WriteLine(chain.Handle(new Request("Noname", "")).Value);
            Console.WriteLine(chain.Handle(new Request("admin", "admin", "admin")).Value);
        }

        public static void TestPipeline()
        {
            PipelineManager Pipeline = new PipelineManager(8);
            Pipeline.SetHandler(0, new PipelineLogHandler());
            Pipeline.SetHandler(2, new PipelineAuthorizeHandler());
            Pipeline.SetHandler(4, new PipelineLogHandler());
            Pipeline.SetHandler(6, new PipelineResponseHandler());

            Pipeline.SetHandler(3, new PipelineIncHandler());
            Console.WriteLine(Pipeline.Handle(new Request("Noname", "")).Value);
            Console.WriteLine(Pipeline.Handle(new Request("admin", "admin")).Value);
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
                new Visa("1234 5678 9012 3456", new DateTime(2032,10,1), -1000),
                new MasterCard("2234 5678 9012 3477", new DateTime(2021,1,1), 5000),
                new MasterCard("3234 5678 9012 3000", new DateTime(2034,12,31), 500),
                new Visa("4234 5678 9012 3456", new DateTime(2033,10,1), 10000),
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
                    Console.WriteLine($"Paid by {card.Number}");
                }
                else
                    Console.WriteLine($"Not paid by {card.Number}");
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

            Console.WriteLine();
            Console.WriteLine("Client: Strategy is set to Capitalize");
            context.SetStrategy(new CapitalizeStrategy());
            context.DoSomeBusinessLogic();
        }

        public static void TestCompositeVisitor()
        {
            Folder folder = new Folder("Project");

            folder.Add(new MyFile("Project", "csproj"))
                .Add(new MyFile("Program", "cs"))
                .Add(new Folder("bin")
                    .Add(new MyFile("Program", "exe"))
                    .Add(new MyFile("config", "json"))
                )
                .Add(new MyFile("", "gitignore"))
                .Add(new MyFile("README", "md"))
                .Add(new Folder("git"));
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
            var collection = new HumanStandartCollection(new Human[4] {
                new Human("Yurii", 32),
                new Human("Andrii", 42),
                new Human("Tetiana", 18),
                new Human("Olekandr", 62)
            });

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

        public static void TestYieldIterator()
        {
            string[] words = new string[] { "First", "Second", "Third" };
            var collection = new IterableCollection<string>(words);
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
        // 1, 10, 8, 12, 2, 3, 7, 4, 6, 11, 5, 
        //1, 8, 12, 2, 3, 4, 7, 6, 5, 11,10
        public static void TestGraphIterator()
        {
            int[,] identityMatrix = new int[13, 13] {
            // https://i.ytimg.com/vi/oDqjPvD54Ss/maxresdefault.jpg
            //   0   1   2   3   4   5   6   7   8   9   10  11 12    
                {0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  1,  0}, //0
                {0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  0}, //1
                {0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  1}, //2
                {0,  0,  1,  0,  1,  0,  0,  1,  0,  0,  0,  0,  0}, //3
                {0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0}, //4
                {0,  0,  0,  0,  0,  0,  1,  0,  0,  0,  0,  0,  0}, //5
                {0,  0,  0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0}, //6
                {0,  0,  0,  1,  0,  0,  1,  0,  0,  0,  0,  1,  0}, //7
                {0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1}, //8
                {0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  0,  0}, //9
                {0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}, //10
                {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}, //11
                {0,  0,  1,  0,  0,  0,  0,  0,  1,  0,  0,  0,  0}, //12
            };
            var graph = new Graph(identityMatrix);
            Console.WriteLine("Default");
            graph.SearchStrategy = new GraphIteratorStrategy();
            foreach (Node node in graph)
                Console.WriteLine(node);
            Console.WriteLine("Breadth-First Search");
            var searchStrategy = new BreadthFirstSearch();
            searchStrategy.StartNode = graph.Nodes[1];
            graph.SearchStrategy = searchStrategy;
            foreach (Node node in graph)
                Console.WriteLine(node);
            Console.WriteLine("Depth-First Search");
            var searchStrategy2 = new DepthFirstSearch();
            searchStrategy2.StartNode = graph.Nodes[1];
            graph.SearchStrategy = searchStrategy2;
            foreach (Node node in graph)
                Console.WriteLine(node);
        }

        public static void TestTemplate()
        {
            var f = new IterativeEquationStrategy(x => (x * x - 2) * (x + 3));
            f.bracketingMethod = new Tabulate();
            f.iterativeMethod = new BinaryDiv();
            var solves = f.Solve();
            Console.WriteLine($"Found {solves.Count} solutions");
            foreach (double root in solves)
            {
                Console.WriteLine(root);
            }

            var f2 = new IterativeEquationTemplateBinaryDiv(x => (x * x - 2) * (x + 3));
            solves = f2.Solve();
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
            // invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));
            invoker.DoSomethingImportant();
        }

        public static void TestInterpreter()
        {
            //2 + 3   2 3 +
            //a * b   a b *
            // Evaluator ex = new Evaluator ("x y z 666 + - +");
            var interpreter = new Interpreter();
            interpreter.Interpret(@"
            x = 2 + 3; 
            y = x + 2;
            z = x + y - 1;
            z + 5"
            );
        }
    }
}