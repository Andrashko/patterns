using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavioral.CompositeVisitor
{
    public interface IVisitable
    {
        void Accept(ICompositeVisitor visitor);
    }

    public interface ICompositeVisitor
    {
        void Visit(MyFile file);
        void Visit(Folder folder);
    }

    public class CompositeComponent : IVisitable
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
            throw new Exception($"Can't remove {Component} to {this}");
        }

        public virtual bool IsComposite { get { return false; } }

        public virtual string ToString(int Level)
        {
            return $"{new String('.', 3 * Level)} {this}\n";
        }

        public virtual void Accept(ICompositeVisitor visitor)
        {
            throw new Exception("Not implemented");
        }
    }

    public class MyFile : CompositeComponent
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

        public override void Accept(ICompositeVisitor visitor)
        {
            visitor.Visit(this);
        }

    }

    public class Folder : CompositeComponent
    {

        public List<CompositeComponent> Elements = new List<CompositeComponent>();
        public Folder(string Name) : base(Name) { }
        public override CompositeComponent Add(CompositeComponent Component)
        {
            Elements.Add(Component);
            return this;
        }

        public override CompositeComponent Remove(CompositeComponent Component)
        {
            Elements.ForEach(Element => this.Remove(Component));
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

        public override void Accept(ICompositeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class SortVisitor : ICompositeVisitor
    {
        public void Visit(MyFile file)
        {
            //Nothing to do
        }
        public void Visit(Folder folder)
        {
            folder.Elements.ToList().ForEach(Element => Element.Accept(this));
            folder.Elements.Sort(
                (Element1, Element2) => String.Compare(Element1.Name, Element2.Name)
            );
        }
    }

    public class FileCountVisitor : ICompositeVisitor
    {
        public int Count = 0;
        public void Visit(MyFile file)
        {
            Count++;
        }
        public void Visit(Folder folder)
        {
            folder.Elements.ForEach(Element => Element.Accept(this));
        }

        public void Reset()
        {
            Count = 0;
        }
    }
}
