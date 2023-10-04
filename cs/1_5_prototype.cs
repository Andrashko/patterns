using System;

namespace Creational
{
    /* 
        приклад шаблону Протоип.
        Ілюструє проблему Deep Copy
    */
    namespace Prototype
    {
        interface IPrototype {
            IPrototype Clone();
        }
        class SomeType: IPrototype
        {
            public string Name = "No name";
            public int Count = 0;

            public IPrototype Clone()
            {
                return this.MemberwiseClone() as SomeType;
            }
        }
        class ProductPrototype: IPrototype
        {
            protected DateTime CreatedAt;
            protected int Id;

            public ProductPrototype() 
            {
                this.CreatedAt = DateTime.Now;
                Random rnd = new Random();
                this.Id = rnd.Next();
            }

            public virtual IPrototype Clone()
            {
                return this.MemberwiseClone() as ProductPrototype;
            }
        }

        class CustomProduct : ProductPrototype
        {
            public SomeType obj;
            public CustomProduct(SomeType obj = null) : base()
            {
                this.obj = obj;
            }
            public override CustomProduct Clone()
            {
                CustomProduct clone = this.MemberwiseClone() as CustomProduct;
                clone.obj = this.obj.Clone() as SomeType;
                clone.CreatedAt = DateTime.Now;
                return clone;
                // якщо використати код, закоментований нижче
                // то виникнуть проблеми  з спільним obj для всіх клонів
                // return this.MemberwiseClone() as CustomProduct; // Deep copy problem
            }

            public override string ToString()
            {
                return $"{{\n\tId:{this.Id},\n\tCreatedAt:{this.CreatedAt},\n\tobj:{{Name: \"{  (this.obj as SomeType).Name}\" }}\n}}";
            }
        }
    }
}