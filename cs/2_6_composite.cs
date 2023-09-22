using System;
using System.Collections.Generic;
using System.Linq;

namespace Structural.Composite
{

    class CompositeComponent 
    {
        public string Name;

        public CompositeComponent(string Name)
        {
            this.Name = Name;
        }
        public virtual CompositeComponent Add(CompositeComponent Component)
        {
            throw new Exception($"Can't add {Component} to {this}");
        }

        public virtual CompositeComponent Remove(CompositeComponent Component)
        {
            throw new Exception($"Can't remove {Component} from {this}");
        }

        public virtual bool IsComposite { get { return false; } }

        public virtual string ToString(int Level)
        {
            return $"{new String('.', 3 * Level)} {this}\n";
        }

        public virtual CompositeComponent Sort()
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
        public override CompositeComponent Add(CompositeComponent Component)
        {
            Elements.Add(Component);
            return this;
        }

        public override CompositeComponent Remove(CompositeComponent Component)
        {
            Elements.Where(Element => Element.IsComposite).ToList()
            .ForEach(Element => Element.Remove(Component));
            Elements.Remove(Component);
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

        public override CompositeComponent Sort()
        {
            Elements.Where(Element => Element.IsComposite).
            ToList().ForEach(Element => Element.Sort());
            Elements.Sort(
                (Element1, Element2) => String.Compare(Element1.Name, Element2.Name)
            );
            return this;
        }

    }
}
