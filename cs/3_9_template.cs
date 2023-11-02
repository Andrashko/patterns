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
        public List<double> Solve(double epsilon = 0.001)
        {
            iterativeMethod.Function = Function;
            bracketingMethod.Function = Function;
            var Roots = new List<double>();
            int Count = bracketingMethod.Separate();
            for (int i = 0; i < Count; i++)
            {
                double Root = iterativeMethod.Refine(bracketingMethod.BrakePoints[2 * i], bracketingMethod.BrakePoints[2 * i + 1], epsilon);
                Roots.Add(Root);
            }
            return Roots;
        }

    }


    public abstract class IterativeMethod
    {
        public Func<double, double> Function;
        public abstract double Refine(double start, double end, double epsilon);
    }

    public class BinaryDiv : IterativeMethod
    {
        public override double Refine(double start, double end, double epsilon)
        {
            double x;
            do
            {
                x = (start + end) / 2;
                if (Function(x) == 0)
                    return x;
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
        public Func<double, double> Function;
        public List<double> BrakePoints;

        public abstract int Separate(double start = -10, double end = 10, double step = 1);
    }

    public class Tabulate : BracketingMethod
    {
        public override int Separate(double start = -10, double end = 10, double step = 1)
        {
            BrakePoints = new List<double>();
            for (double i = start; i < end; i += step)
            {
                if (Function(i) * Function(i + 1) < 0)
                {
                    BrakePoints.Add(i);
                    BrakePoints.Add(i + 1);
                }

                if (Function(i) == 0)
                {
                    BrakePoints.Add(i);
                    BrakePoints.Add(i);
                }
            }
            return BrakePoints.Count / 2;
        }
    }
}