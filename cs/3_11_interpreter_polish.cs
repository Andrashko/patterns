using System;
using System.Collections.Generic;

namespace Behavioral.InterpreterPolish
{
    public interface IExpression
    {
        string Name { get; }
        int Interpret(List<IExpression> Context);
    }

    public class Number : IExpression
    {
        private int Value;

        public string Name { get { return Value.ToString(); } }
        public Number(int Value)
        {
            this.Value = Value;
        }
        public int Interpret(List<IExpression> Context)
        {
            return Value;
        }
    }

    public class Operation
    {
        protected IExpression Left;
        protected IExpression Right;
        public Operation(IExpression Left, IExpression Righ)
        {
            this.Left = Left;
            this.Right = Righ;
        }
    }

    public class Plus : Operation, IExpression
    {
        public string Name { get { return "+"; } }
        public Plus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public int Interpret(List<IExpression> Variables)
        {
            return Left.Interpret(Variables) + Right.Interpret(Variables);
        }
    }

    public class Minus : Operation, IExpression
    {
        public string Name { get { return "-"; } }
        public Minus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public int Interpret(List<IExpression> Variables)
        {
            return Left.Interpret(Variables) - Right.Interpret(Variables);
        }
    }

    public class Variable : IExpression
    {
        public string Name { get; set; }

        public Number Value { get; set; }
        public Variable(string Name, Number Value = null)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public int Interpret(List<IExpression> Context)
        {
            Variable variable = Context.Find(
                v => v.Name == this.Name
            ) as Variable;
            if (variable == null)
                return 0;
            if (variable.Value == null)
                throw new Exception("Value is not assigned");
            return variable.Value.Interpret(Context);
        }
    }

    public class Evaluator : IExpression
    {
        IExpression Tree;

        public string Name { get; set; }
        public Evaluator(string Expression)
        {
            Name = Expression;
            Stack<IExpression> expressionStack = new Stack<IExpression>();
            int next = 0;
            int last = 0;
            while (last < Expression.Length)
            {
                next = Expression.IndexOf(" ", last);
                if (next == -1)
                    next = Expression.Length;
                string token = Expression.Substring(last, next - last);
                if (token == "+")
                {
                    IExpression right = expressionStack.Pop();
                    IExpression left = expressionStack.Pop();
                    IExpression subexpression = new Plus(left, right);
                    expressionStack.Push(subexpression);
                }
                else if (token == "-")
                {
                    IExpression right = expressionStack.Pop();
                    IExpression left = expressionStack.Pop();
                    IExpression subexpression = new Minus(left, right);
                    expressionStack.Push(subexpression);
                }
                else if (int.TryParse(token, out _))
                {
                    expressionStack.Push(new Number(int.Parse(token)));
                }
                else
                {
                    expressionStack.Push(new Variable(token));
                }

                last = next + 1;
            }
            Tree = expressionStack.Pop();
        }

        public int Interpret(List<IExpression> context)
        {
            return Tree.Interpret(context);
        }
    }

}