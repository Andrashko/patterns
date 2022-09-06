using System;
using System.Collections.Generic;
using System.Linq;

namespace Structural.Composite
{

    interface ICompositeComponent
    {
        ICompositeComponent Add(ICompositeComponent Component);

        ICompositeComponent Remove(ICompositeComponent Component);

        bool IsComposite { get; }

    }

    class CompositeComponent : ICompositeComponent
    {
        public string Name;

        public CompositeComponent(string Name)
        {
            this.Name = Name;
        }
        public virtual ICompositeComponent Add(ICompositeComponent Component)
        {
            throw new Exception($"Can't add {Component} to {this}");
        }

        public virtual ICompositeComponent Remove(ICompositeComponent Component)
        {
            throw new Exception($"Can't remove {Component} to {this}");
        }

        public virtual bool IsComposite { get { return false; } }

        public virtual string ToString(int Level)
        {
            return $"{new String('.', 3 * Level)} {this}\n";
        }

        public virtual ICompositeComponent Sort()
        {
            return this;
        }
    }

    class MyFile : CompositeComponent
    {
        public string Ext;

        public MyFile(string Name, string Ext) : base(Name)
        {
            this.Ext = Ext;
        }

        public override string ToString()
        {
            return $"{Name}.{Ext}";
        }

    }

    class Folder : CompositeComponent
    {

        private List<CompositeComponent> Elements = new List<CompositeComponent>();
        public Folder(string Name) : base(Name) { }
        public override ICompositeComponent Add(ICompositeComponent Component)
        {
            Elements.Add(Component as CompositeComponent);
            return this;
        }

        public override ICompositeComponent Remove(ICompositeComponent Component)
        {
            Elements.ForEach(Element => this.Remove(Element));
            Elements.Remove(Component as CompositeComponent);
            return this;
        }

        public override bool IsComposite { get { return true; } }

        public override string ToString(int Level)
        {
            string Result = $"{new String('.', 3 * Level)} > {this}\n";
            foreach (CompositeComponent Element in Elements)
            {
                Result += Element.ToString(Level + 1);
            }
            return Result;
        }

         public override string ToString()
        {
            return Name;
        }

        public override ICompositeComponent Sort()
        {
            Elements.Where(Element => Element.IsComposite).ToList().ForEach(Element => Element.Sort());
            Elements.Sort(
                (Element1, Element2) => String.Compare(Element1.Name, Element2.Name)
            );
            return this;
        }

    }
}
