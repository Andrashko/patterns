namespace Structural.Bridge
{
    interface IImplementation
    {
        string OperationImplementation();
    }
    class Abstraction
    {
        protected IImplementation _implementation;
        public Abstraction(IImplementation implementation)
        {
            this._implementation = implementation;
        }
        public virtual string Operation()
        {
            return "Abstract: Base operation with:\n" +
            _implementation.OperationImplementation();
        }
    }
    class ExtendedAbstraction : Abstraction
    {
        public ExtendedAbstraction(IImplementation implementation) : base(implementation)
        {
        }
        public override string Operation()
        {
            return "ExtendedAbstraction: Extended operation with:\n" +
            base._implementation.OperationImplementation();
        }
    }

    class ConcreteImplementationA : IImplementation
    {
        public string OperationImplementation()
        {
            return "ConcreteImplementationA: The result in platform A.\n";
        }
    }
    class ConcreteImplementationB : IImplementation
    {
        public string OperationImplementation()
        {
            return "ConcreteImplementationA: The result in platform B.\n";
        }
    }

}