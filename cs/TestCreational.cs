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
            Singleton s2 = Singleton.getInstance();
            s1.Print();
            s2.Print();
            s1.IncCounter();
            s2.IncCounter();
            s1.IncCounter();
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

        public static void TestFabricMethod()
        {
            Console.Write("Enter products number:");
            int count = int.Parse(Console.ReadLine());
            Console.Write("Select the product type A, B or C:");
            string choise = Console.ReadLine();
            ICreator productCreator;
            if (choise == "A")
                productCreator = new ProductACreator();
            else if (choise == "B")
                productCreator = new ProductBCreator();
            else if (choise == "C")
                productCreator = new ProductCCreator();
            else
                throw new Exception("Wrong product type");
            List<IProduct> productList = productCreator.CreateProductList(count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(productList[i].Operation());
            }

            IProduct p = productCreator.CreateProductByName(choise );
        }

        public static void TestAbstractFabric()
        {
            Console.Write("Select category first or second:");
            int category = int.Parse(Console.ReadLine());
            IAbstractFactory factory;
            if (category == 1)
                factory = new FactoryFirstClass();
            else
                factory = new FactorySecondClass();
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
        }

        public static void TestPrototype()
        {
            var p = new SomeType();
            CustomProduct product = new CustomProduct(p);
            CustomProduct productClone = product.Clone();
            (productClone.obj as SomeType).Name = "New name";
            Console.WriteLine(product.ToString());
            Console.WriteLine(productClone.ToString());
        }
     }
}