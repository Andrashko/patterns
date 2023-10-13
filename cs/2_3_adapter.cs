using System;

namespace Structural.Adapter
{
    public interface ITarget
    {
        string GetRequest(string Request);
    }

    class Adaptee
    {
        public string GetSpecificRequest(string Request, int RequestLength, bool MustBeTrue)
        {
            if (!MustBeTrue)
                throw new Exception("MustBeTrue is not true");
            if (Request.Length != RequestLength)
                throw new Exception("RequestLength is wrong");
            return $"Response for {Request}";
        }
    }

    class Adapter : ITarget
    {
        private readonly Adaptee _adaptee;
        public Adapter(Adaptee adaptee)
        {
            this._adaptee = adaptee;
        }

        public string GetRequest(string Request)
        {
            return $"This is adapted '{this._adaptee.GetSpecificRequest(Request, Request.Length, true)}'";
        }
    }
}