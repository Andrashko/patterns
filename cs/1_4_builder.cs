using System.Collections.Generic;
using System;

namespace Creational
{
    /* 
        Приклад шаблону Будывельник.
        Визначено такі етапи побудови:
        1. Додавання частини
        2. Додавання часової мітки
        3. Зміна імені
    */ 
    namespace Builder
    {
        class Product
        {
            private List<object> parts = new List<object>();
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
            IBuilder AddPart(object part);
            IBuilder SetDateStemp();
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

            public IBuilder AddPart (object part)
            {
                this.product.Add(part as string);
                return this;
            }

            public virtual IBuilder SetDateStemp ()
            {
                this.product.Add($"Date stemp: {DateTime.Now.ToString()}");
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
            public override IBuilder SetDateStemp ()
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
                        .SetDateStemp()
                        .AddPart("Part Three")
                        .GetProduct();
            }
        }
    }
}