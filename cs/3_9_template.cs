using System;
using System.Collections.Generic;

namespace Behavioral.Template
{
    public class Equation
    {

        private Func<double, double> Function;
        public IterativeMethod iterativeMethod;
        public BracketingMethod bracketingMethod;
        public Equation(Func<double, double> Function)
        {
            this.Function = Function;
        }
        public List<double> Solve()
        {
            iterativeMethod = new BinaryDiv(Function);
            bracketingMethod = new Tabulate(Function);
            var Roots = new List<double>();
            int Count = bracketingMethod.Separate();
            for (int i = 0; i < Count; i++)
            {
                double Root = iterativeMethod.Refine(bracketingMethod.BrakePoints[2 * i], bracketingMethod.BrakePoints[2 * i + 1], 0.001);
                Roots.Add(Root);
            }
            return Roots;
        }

    }


    public abstract class IterativeMethod
    {
        protected Func<double, double> Function;
        public IterativeMethod(Func<double, double> Function)
        {
            this.Function = Function;
        }

        public abstract double Refine(double start, double end, double epsilon);
    }

    public class BinaryDiv : IterativeMethod
    {
        public BinaryDiv(Func<double, double> Function) : base(Function) { }
        public override double Refine(double start, double end, double epsilon)
        {
            double x;
            do
            {
                x = (start + end) / 2;
                if (Function(start) * Function(x) < 0)
                    end = x;
                else
                    start = x;

            } while (Math.Abs(end - start) > epsilon);
            return x;
        }
    }

    public abstract class BracketingMethod
    {
        protected Func<double, double> Function;
        public BracketingMethod(Func<double, double> Function)
        {
            this.Function = Function;
        }

        public List<double> BrakePoints;

        public abstract int Separate();
    }

    public class Tabulate : BracketingMethod
    {
        public Tabulate(Func<double, double> Function) : base(Function) { }

        public override int Separate()
        {
            BrakePoints = new List<double>();
            for (int i = -10; i < 10; i++)
            {
                if (Function(i) * Function(i + 1) < 0)
                {
                    BrakePoints.Add(i);
                    BrakePoints.Add(i + 1);
                }
            }
            return BrakePoints.Count / 2;
        }
    }
}