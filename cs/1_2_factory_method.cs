using System.Collections.Generic;
using System;

namespace Creational
{
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

            
            public static IProduct CreateProductByName(string Name){
                if (Name == "A")
                    return new ProductA();
                if (Name == "B")
                    return new ProductВ();
                if (Name == "C")
                    return new ProductC();
                throw new Exception("Wrong type");
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