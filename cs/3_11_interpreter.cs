using System;
using System.Collections.Generic;

namespace Behavioral.Interpreter
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

    public class Operation : IExpression
    {
        protected IExpression Left;
        protected IExpression Right;

        public virtual string Name { get; }
        public Operation() { }
        public Operation(IExpression Left, IExpression Righ)
        {
            this.Left = Left;
            this.Right = Righ;
        }
        public virtual int Interpret(List<IExpression> Context)
        {
            throw new Exception();
        }
        public virtual Operation Construct(IExpression Left, IExpression Righ)
        {
            return new Operation(Left, Righ);
        }
    }

    public class Plus : Operation
    {
        public override string Name { get { return "+"; } }
        public Plus() : base() { }
        public Plus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            return Left.Interpret(Context) + Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Righ)
        {
            return new Plus(Left, Righ);
        }
    }

    public class Minus : Operation
    {
        public override string Name { get { return "-"; } }
        public Minus() : base() { }
        public Minus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            return Left.Interpret(Context) - Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Righ)
        {
            return new Minus(Left, Righ);
        }
    }

    public class Split : Operation
    {
        public override string Name { get { return ";"; } }
        public Split() : base() { }
        public Split(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            Left.Interpret(Context);
            return Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Righ)
        {
            return new Split(Left, Righ);
        }
    }

    public class Assign : Operation
    {
        public override string Name { get { return "="; } }

        public Assign() : base() { }
        public Assign(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            if (!(Left is Variable))
                throw new Exception("assign only to variable!");
            if (!(Right is Number))
                throw new Exception("Only numbers!");
            (Left as Variable).Value = (Right as Number);
            Variable variable = Context.Find(
                v => v.Name == (Left as Variable).Name
            ) as Variable;
            if (variable == null)
                Context.Add(Left);
            else
                (variable as Variable).Value = Right as Number;
            return Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Righ)
        {
            return new Assign(Left, Righ);
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
            return (variable).Value.Interpret(Context);
        }
    }

    public class Evaluator : IExpression
    {
        private IExpression Tree;
        private List<Operation> Operations = new List<Operation>(){
            new Plus(),
            new Minus(),
            new Assign(),
            new Split()
        };

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
                bool executeOp = expressionStack.Count > 0 && expressionStack.Peek() is Operation ;

                Operation op = Operations.Find(op => op.Name == token);
                if (op != null)
                {
                    executeOp =  false;
                    expressionStack.Push(op);
                }
                else if (int.TryParse(token, out _))
                {
                    expressionStack.Push(new Number(int.Parse(token)));
                }
                else
                {
                    expressionStack.Push(new Variable(token));
                }
                if (executeOp)
                {
                    IExpression right = expressionStack.Pop();
                    Operation operation = expressionStack.Pop() as Operation;
                    IExpression left = expressionStack.Pop();
                    expressionStack.Push(operation.Construct(left, right));
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