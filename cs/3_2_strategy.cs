using System;
using System.Collections.Generic;

namespace Behavioral.Strategy
{
    class Context
    {
        private IStrategy<string> _strategy;

        public Context()
        { }

        public Context(IStrategy<string> strategy)
        {
            this._strategy = strategy;
        }
        public void SetStrategy(IStrategy<string> strategy)
        {
            this._strategy = strategy;
        }

        public void DoSomeBusinessLogic()
        {
            Console.WriteLine("Context: Change data using the strategy");
            List<string> data = new List<string> { "a", "b", "e", "c", "d", };
            List<string> result = this._strategy.DoAlgorithm(data);

            string resultStr = String.Join(",", result);

            Console.WriteLine(resultStr);
        }
    }

    public interface IStrategy<T>
    {
        List<T> DoAlgorithm(List<T> data);
    }

    class ConcreteStrategyA : IStrategy<string>
    {
        public List<string> DoAlgorithm(List<string> data)
        {
            data.Sort();
            return data;
        }
    }

    class ConcreteStrategyB : IStrategy<string>
    {
        public List<string> DoAlgorithm(List<string> data)
        {
            data.Sort();
            data.Reverse();
            return data;
        }
    }

    class CapitalizeStrategy : IStrategy<string>
    {
        public List<string> DoAlgorithm(List<string> data)
        {
            for (int i = 0; i < data.Count; i++)
                data[i] = data[i].ToUpper();
            return data;
        }
    }
}