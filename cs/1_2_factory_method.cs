using System.Collections.Generic;
using System;

namespace Creational
{
    /*
    приклад фабричного методу
    */
    namespace FactoryMethod
    {
        interface IProduct
        {
            string Operation();
        }

        abstract class ICreator
        {
            public abstract IProduct CreateProduct();

            public List<IProduct> CreateProductList(int count)
            {
                List<IProduct> productList = new List<IProduct>(count);
                for (int i = 0; i < count; i++)
                {
                    productList.Add(this.CreateProduct());
                }
                return productList;
            }

            //даний метод не є частиною GoF шаблону
            // але в сучасному програмуванні часто фабричний метод використовується 
            // як статичний метод класу для створення.
            // код цього методу може бути покращено використанням шаблону Стратегія замість if 
            public static Dictionary<string, ICreator> creationStrategies = new Dictionary<string, ICreator>{
                    {"A", new ProductACreator()},
                    {"B", new ProductBCreator()},
                    {"C", new ProductCCreator()},
                };
            public static IProduct CreateProductByName(string Name)
            {
                // if (Name == "A")
                //     return new ProductA();
                // if (Name == "B")
                //     return new ProductВ();
                // if (Name == "C")
                //     return new ProductC();
                // return null;

                if (!creationStrategies.ContainsKey(Name))
                    throw new Exception("Wrong type");
                return creationStrategies.GetValueOrDefault(Name).CreateProduct();
            }
        }

        class ProductA : IProduct
        {
            public string Operation()
            {
                return "This is A";
            }
        }
        class ProductВ : IProduct
        {
            public string Operation()
            {
                return "This is B";
            }
        }

        class ProductC : IProduct
        {
            public string Operation()
            {
                return "This is C";
            }
        }

        class ProductACreator : ICreator
        {
            public override IProduct CreateProduct()
            {
                return new ProductA();
            }
        }

        class ProductBCreator : ICreator
        {
            public override IProduct CreateProduct()
            {
                return new ProductВ();
            }

        }

        class ProductCCreator : ICreator
        {
            public override IProduct CreateProduct()
            {
                return new ProductC();
            }
        }

    }
}