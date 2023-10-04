using System.Collections.Generic;
using System;

namespace Creational
{
    /* 
        Приклад шаблону Будівельник.
        Визначено такі етапи побудови:
        1. Додавання частини
        2. Додавання часової мітки
        3. Зміна імені
    */ 
    namespace Builder
    {
        class Product
        {
            private List<string> parts = new List<string>();
            public string Name = "No name";
            public void Add(string part)
            {
                this.parts.Add(part);
            }
            
            public override string ToString ()
            {
                string str = string.Empty;
                foreach (string part in this.parts)
                {
                    str += $"\t{part},\n ";
                }
                return $"Product <{this.Name}> parts: \n {str}";
            }
        }
        
        interface IBuilder
        {
            IBuilder AddPart(string part);
            IBuilder SetDateStamp();
            IBuilder SetName(string name);
            Product GetProduct();
            void Reset();
        }

        class Builder : IBuilder
        {
            protected Product product  = new Product();

            public void Reset()
            {
                this.product = new Product();
            }
            public IBuilder SetName(string name)
            {
                this.product.Name = name;
                return this; //для можливості побудови ланцюга виклику методів 
            }

            public IBuilder AddPart (string part)
            {
                this.product.Add(part);
                return this;
            }

            public virtual IBuilder SetDateStamp ()
            {
                this.product.Add($"Date stamp: {DateTime.Now.ToString()}");
                return this;
            }

            public Product GetProduct()
            {
                Product result = this.product;
                this.Reset();
                return result;
            }
        }

        class OtherBuilder : Builder {
            public override IBuilder SetDateStamp ()
            {
                this.product.Add($"{DateTime.Now.ToString()}");
                return this;
            }
        }
        /* клас Директор реалізує шаблон Фабричний метод 
        та генерує деякі пресети з використанням Будівельника
        */ 
        class Director
        {
            public IBuilder builder;
            public Director(IBuilder builder)
            {
                this.builder = builder;
            }

            public Product Empty()
            {
                this.builder.Reset();
                return this.builder.GetProduct();
            }

            public Product BuildFromParts (string[] parts)
            {
                this.builder.Reset();
                foreach (string part in parts)
                {
                    this.builder.AddPart(part);
                }
                return this.builder.GetProduct();
            }

            public Product Example()
            {
                this.builder.Reset();
                return this.builder
                        .SetName("Example")
                        .AddPart("Part One")
                        .AddPart("Part Two")
                        .SetDateStamp()
                        .AddPart("Part Three")
                        .GetProduct();
            }
        }
    }
}