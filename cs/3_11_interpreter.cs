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
        public int Priority { get; set; } = 0;
        public IExpression Left;
        public IExpression Right;

        public virtual string Name { get; }
        public Operation() { }
        public Operation(IExpression Left, IExpression Right)
        {
            this.Left = Left;
            this.Right = Right;
        }
        public virtual int Interpret(List<IExpression> Context)
        {
            throw new Exception();
        }
        public virtual Operation Construct(IExpression Left, IExpression Right)
        {
            return new Operation(Left, Right);
        }
    }

    public class Plus : Operation
    {
        public override string Name { get { return "+"; } }
        public Plus() : base()
        {
            Priority = 1;
        }
        public Plus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            return Left.Interpret(Context) + Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Right)
        {
            return new Plus(Left, Right);
        }
    }

    public class Minus : Operation
    {
        public override string Name { get { return "-"; } }
        public Minus() : base()
        {
            Priority = 1;
        }
        public Minus(IExpression Left, IExpression Right) : base(Left, Right) { }
        public override int Interpret(List<IExpression> Context)
        {
            return Left.Interpret(Context) - Right.Interpret(Context);
        }

        public override Operation Construct(IExpression Left, IExpression Right)
        {
            return new Minus(Left, Right);
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
            int val = Right.Interpret(Context);
            var Value = new Number(val);
            (Left as Variable).Value = Value;
            Variable variable = Context.Find(
                v => v.Name == (Left as Variable).Name
            ) as Variable;
            if (variable == null)
                Context.Add(Left);
            else
                variable.Value = (Left as Variable).Value;
            return val;
        }

        public override Operation Construct(IExpression Left, IExpression Right)
        {
            return new Assign(Left, Right);
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
                v => v.Name == Name
            ) as Variable;
            if (variable == null || variable.Value == null)
                throw new Exception("Value is not assigned");
            return variable.Value.Interpret(Context);
        }
    }

    public class Interpreter
    {
        private List<IExpression> context = new List<IExpression>();
        private List<Operation> Operations = new List<Operation>(){
            new Plus(),
            new Minus(),
            new Assign(),
        };

        public IExpression ParseOperation(string Expression)
        {
            Stack<IExpression> expressionStack = new Stack<IExpression>();
            int next = 0;
            int last = 0;
            while (last < Expression.Length)
            {
                next = Expression.IndexOf(" ", last);
                if (next == -1)
                    next = Expression.Length;
                string token = Expression.Substring(last, next - last);
                bool executeOperation = expressionStack.Count > 0 && expressionStack.Peek() is Operation;
                int val;
                Operation op = Operations.Find(op => op.Name == token);
                if (op != null)
                {
                    executeOperation = false;
                    expressionStack.Push(op);
                }
                else if (int.TryParse(token, out val))
                {
                    expressionStack.Push(new Number(val));
                }
                else
                {
                    expressionStack.Push(new Variable(token));
                }

                if (executeOperation)
                {
                    IExpression right = expressionStack.Pop();
                    Operation operation = expressionStack.Pop() as Operation;
                    IExpression left = expressionStack.Pop();
                    if (left is Operation && (left as Operation).Priority < operation.Priority)
                    {
                        var highPriorityOperation = operation.Construct((left as Operation).Right, right);
                        (left as Operation).Right = highPriorityOperation;
                        expressionStack.Push(left);
                    }
                    else
                        expressionStack.Push(operation.Construct(left, right));
                }


                last = next + 1;
            }
            return expressionStack.Pop();
        }

        public void Interpret(string program)
        {
            foreach (string operation in program.Split(";"))
            {
                var commandsTree = ParseOperation(operation.Trim());
                int result = commandsTree.Interpret(context);
                Console.WriteLine($"{operation.Trim()} has been interpreted. The result is {result}");
            }
        }
    }

}