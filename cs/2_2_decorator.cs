namespace Structural.Decorator
{
    /* 
    приклад шаблону Декоратор 
    */
    interface IComponent
    {
        public abstract string Operation();
    }

    class ConcreteComponent : IComponent
    {
        public string Operation()
        {
            return "Concrete Component";
        }
    }

    class Decorator : IComponent
    {
        protected IComponent _component;
        public Decorator(IComponent component)
        {
            this._component = component;
        }
        public virtual string Operation()
        {
            return this._component.Operation();
        }
       
        public IComponent Undecorate(){
            return this._component;
        }
    }
    class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(IComponent comp) : base(comp)
        {
        }
        public override string Operation()
        {
            return $"ConcreteDecoratorA({base.Operation()})";
        }

        public int OtherOperation()
        {
            return base.Operation().Length;
        }
    }

    class ConcreteDecoratorB : Decorator
    {
        public ConcreteDecoratorB(IComponent comp) : base(comp)
        {
        }

        public override string Operation()
        {
            return $"ConcreteDecoratorB({base.Operation()})";
        }
    }
}
