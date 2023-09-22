namespace Creational
{
    /*
    приклад шаблону Фабрика
    */
    namespace AbstractFactory
    {
        interface IProductA
        {
            string OperationA();
        }
        interface IProductB
        {
            string OperationB();
            string OperationWithProductA(IProductA product);
        }

        interface IAbstractFactory
        {
            IProductA CreateProductA();
            IProductB CreateProductB();
        }

        class ProductAFirst : IProductA
        {
            public string OperationA()
            {
                return "This is A of first class";
            }
        }

        class ProductASecond : IProductA
        {
            public string OperationA()
            {
                return "This is A of second class";
            }
        }

        class ProductBFirst : IProductB
        {
            public string OperationB()
            {
                return "This is B of first class";
            }

            public string OperationWithProductA(IProductA product)
            {
                return $"{this.OperationB()} AND {product.OperationA()}";
            }
        }

        class ProductBSecond : IProductB
        {
            public string OperationB()
            {
                return "This is B of second class";
            }

            public string OperationWithProductA(IProductA product)
            {
                return $"{this.OperationB()} AND {product.OperationA()}";
            }
        }

        class FactoryFirstClass : IAbstractFactory
        {
            public IProductA CreateProductA()
            {
                return new ProductAFirst();
            }

            public IProductB CreateProductB()
            {
                return new ProductBFirst();
            }
        }

        class FactorySecondClass : IAbstractFactory
        {
            public IProductA CreateProductA()
            {
                return new ProductASecond();
            }

            public IProductB CreateProductB()
            {
                return new ProductBSecond();
            }
        }
    }
}