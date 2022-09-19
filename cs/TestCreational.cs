using System;
using System.Collections.Generic;
using Creational.Singleton;
using Creational.FactoryMethod;
using Creational.AbstractFactory;
using Creational.Builder;
using Creational.Prototype;
namespace Test
{
    class CreationalPaterns
    {
        public static void TestSingleton()
        {
            Singleton s1 = Singleton.getInstance();
            s1.IncCounter();
            Singleton s2 = Singleton.getInstance();
            s1.Print();
            s2.Print();
            s1.IncCounter();
            s2.IncCounter();
            s1.IncCounter();
            s1.Print();
            s2.Print();
        }

        public static void TestLogSystem()
        {
            var log1 = LogSystem.getInstance();
            var log2 = LogSystem.getInstance();
            log1.Log("Test");
            log2.Log("Hello world");
            log1.ShowLog();
            log2.ShowLog();
        }

        private static ICreator SelectCreator()
        {
            Console.Write("Select the product type A, B or C:");
            string choise = Console.ReadLine();
            if (choise == "A")
                return new ProductACreator();
            if (choise == "B")
                return new ProductBCreator();
            else if (choise == "C")
                return new ProductCCreator();
            throw new Exception("Wrong product type");
        }
        public static void TestFabricMethod()
        {
            ICreator productCreator = SelectCreator();
            Console.WriteLine("One product of selected type");
            Console.WriteLine(productCreator.CreateProduct().Operation());
            Console.Write("Enter products number:");
            int count = int.Parse(Console.ReadLine());
            Console.WriteLine("List of products of selected type");
            List<IProduct> productList = productCreator.CreateProductList(count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(productList[i].Operation());
            }
            Console.WriteLine("And one more  by method CreateProductByName()");
            IProduct p = ICreator.CreateProductByName("A");
            Console.WriteLine(p.Operation());
        }

        private static IAbstractFactory SelectFactory(){
            Console.Write("Select category first or second:");
            int category = int.Parse(Console.ReadLine());
            if (category == 1)
                return  new FactoryFirstClass();
            else
                return new FactorySecondClass();
        }
        public static void TestAbstractFabric()
        {
            IAbstractFactory factory = SelectFactory();        
            IProductA productA = factory.CreateProductA();
            IProductB productB = factory.CreateProductB();
            Console.WriteLine(productA.OperationA());
            Console.WriteLine(productB.OperationB());
            Console.WriteLine(productB.OperationWithProductA(productA));
        }

        public static void TestBuilder()
        {
            IBuilder builder = new Builder();
            Product product = builder
                                .SetName("Custom product")
                                .SetDateStemp()
                                .AddPart("Part One")
                                .SetDateStemp()
                                .AddPart("Part Two")
                                .SetDateStemp()
                                .AddPart("Part Three")
                                .GetProduct();
            Console.WriteLine(product.ToString());
            Director director = new Director(builder);
            Console.WriteLine(director.Empty().ToString());
            Console.WriteLine(director.Example().ToString());
            string[] parts = new string[3] { "One", "Two", "Tree" };
            Console.WriteLine(director.BuildFromParts(parts).ToString());

            director.builder = new OtherBuilder();
            Console.WriteLine(director.Example().ToString());
        }

        public static void TestPrototype()
        {
            var obj = new SomeType();
            CustomProduct product = new CustomProduct(obj);
            System.Threading.Thread.Sleep(2000);
            CustomProduct productClone = product.Clone();
            (productClone.obj as SomeType).Name = "New name";
            Console.WriteLine(product.ToString());
            Console.WriteLine(productClone.ToString());
        }
    }
}