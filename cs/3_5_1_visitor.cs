using System;
using System.Collections.Generic;

namespace Behavioral.Visitor
{

    public interface IComponent
    {
        void Accept(IVisitor visitor);
    }


    public class ConcreteComponentA : IComponent
    {
        public void Accept(IVisitor visitor)
        {
            visitor.VisitConcreteComponent(this);
        }

        public string ExclusiveMethodOfConcreteComponentA()
        {
            return "A";
        }
    }

    public class ConcreteComponentB : IComponent
    {
        public void Accept(IVisitor visitor)
        {
            visitor.VisitConcreteComponent(this);
        }

        public string SpecialMethodOfConcreteComponentB()
        {
            return "B";
        }
    }

    public interface IVisitor
    {
        void VisitConcreteComponent(ConcreteComponentA element);

        void VisitConcreteComponent(ConcreteComponentB element);
    }

    class ConcreteVisitor1 : IVisitor
    {
        public void VisitConcreteComponent(ConcreteComponentA element)
        {
            Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor1");
        }

        public void VisitConcreteComponent(ConcreteComponentB element)
        {
            Console.WriteLine(element.SpecialMethodOfConcreteComponentB() + " + ConcreteVisitor1");
        }
    }

    class ConcreteVisitor2 : IVisitor
    {
        public void VisitConcreteComponent(ConcreteComponentA element)
        {
            Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor2");
        }

        public void VisitConcreteComponent(ConcreteComponentB element)
        {
            Console.WriteLine(element.SpecialMethodOfConcreteComponentB() + " + ConcreteVisitor2");
        }
    }

    public class Client
    {

        public static void ClientCode(List<IComponent> components, IVisitor visitor)
        {
            foreach (var component in components)
            {
                component.Accept(visitor);
            }
        }
    }
}