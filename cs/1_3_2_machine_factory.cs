
using System;
using System.Collections.Generic;

namespace Creational
{
    /*
    приклад фабричного методу
    */
    namespace Mix
    {
        //  Команда машини, яку можна виконати 
        interface IExecutable
        {
            void Execute();
        }
        // машина 
        abstract class Machine
        {
            protected Machine() { }
            public static Machine getInstance()
            {
                throw new Exception("Abstract method");
            }

            public List<IExecutable> program;

            //реалізація виконання програми неповна, адже не враховуює регістр команд тому не реалізовано можливості умовного та безумовного переходу
            public void ExecuteProgram()
            {

                foreach (IExecutable operation in program)
                {
                    operation.Execute();
                }
            }
        }

        //=========================================== mix ==========================
        // часткова реалізація mix машини з одним регістром RA як синглтон
        class MixMachine : Machine
        {
            public int RA = 0;

            protected static MixMachine _instance = null;
            private MixMachine() : base() { }


            public new static MixMachine getInstance()
            {
                if (_instance == null)
                {
                    _instance = new MixMachine();
                }
                return _instance;
            }

            public override string ToString()
            {
                return $"RA:{RA}";
            }
        }

        // оператор машини mix може мати тыльки 1 операнд

        abstract class MixOperator : IExecutable
        {
            protected string operand;
            protected MixMachine mixMachine = MixMachine.getInstance();
            public MixOperator(string operand)
            {
                this.operand = operand;
            }

            public MixOperator()
            {
                this.operand = String.Empty;
            }
            public abstract void Execute();
        }
        // реалізація конкретних операторів машини
        class RDRA : MixOperator
        {
            public override void Execute()
            {
                Console.Write("Please enter value to RA:");
                string input = Console.ReadLine();
                int argument = int.Parse(input);
                mixMachine.RA = argument;
            }
        }

        class WRRA : MixOperator
        {
            public override void Execute()
            {
                Console.WriteLine($"RA value is {mixMachine.RA}");
            }
        }

        class INCA : MixOperator
        {
            public INCA(string argument) : base(argument) { }
            public override void Execute()
            {

                int argument = int.Parse(operand);
                mixMachine.RA += argument;

            }
        }

        class STOP : MixOperator
        {
            public override void Execute()
            {
                Console.WriteLine("Stop operator. Press Enter ");
                Console.ReadLine();
                // Environment.Exit(0);
            }
        }

        /*================= ASM ===================*/
        // часткова реалізація асемблера, тільки 2 регістри та операція mov
        class AsmMachine : Machine
        {
            public byte ah = 0;
            public byte bh = 0;

            protected static AsmMachine _instance = null;
            private AsmMachine() : base() { }


            public new static AsmMachine getInstance()
            {
                if (_instance == null)
                {
                    _instance = new AsmMachine();
                }
                return _instance;
            }

            public override string ToString()
            {
                return $"ah:{ah}\nbh:{bh}";
            }
        }

        //оператор може може мати 2 операнди
        abstract class AsmOperator : IExecutable
        {
            protected string firstOperand;
            protected string secondOperand;
            protected AsmMachine asmMachine = AsmMachine.getInstance();
            public AsmOperator(string firstOperand, string secondOperand)
            {
                this.firstOperand = firstOperand;
                this.secondOperand = secondOperand;
            }

            public abstract void Execute();
        }
        //опертор mov копіює дані в джерело, наприклад "mov ah, 5" скопіює до регістра ah число 5.
        class MOV : AsmOperator
        {
            public MOV(string firstOperand, string secondOperand) : base(firstOperand, secondOperand) { }
            public override void Execute()
            {
                if (firstOperand == "ah")
                    asmMachine.ah = byte.Parse(secondOperand);
                if (firstOperand == "bh")
                    asmMachine.bh = byte.Parse(secondOperand);
                ///... тут має бути складна логіка реалізації
            }
        }

        //опертор add додає другий операнд до першого, наприклад "add ah, 5" додасть до регістра ah число 5.
        class ADD : AsmOperator
        {
            public ADD(string firstOperand, string secondOperand) : base(firstOperand, secondOperand) { }
            public override void Execute()
            {
                if (firstOperand == "ah")
                    asmMachine.ah += byte.Parse(secondOperand);
                if (firstOperand == "bh")
                    asmMachine.bh += byte.Parse(secondOperand);
                ///... тут має бути складна логіка реалізації
            }
        }


        /*================= фабричний метод ===============*/
        //створє опертаор із рядка
        interface IOperatorFactoryMethod
        {
            IExecutable CreateOperator(string Line);
        }
        // створює оператор для mix машини
        class MixOperatorFactoryMethod : IOperatorFactoryMethod
        {
            public IExecutable CreateOperator(string Line)
            {
                var parts = Line.Trim().Split();
                string operation = parts[0].ToUpper();
                string operand = null;


                if (parts.Length > 1)
                    operand = parts[1];
                //тут порушується принцип  open-close. Для виправленння коду потрібно використати шаблон "стратегія"
                if (operation == "RDRA")
                    return new RDRA();
                if (operation == "WRRA")
                    return new WRRA();
                if (operation == "INCA")
                    return new INCA(operand);
                if (operation == "STOP")
                    return new STOP();
                throw new Exception($"unknown operation {operation}");
            }
        }

        // створює опертаор для асемблер машини
        class AsmOperatorFactoryMethod : IOperatorFactoryMethod
        {
            public IExecutable CreateOperator(string Line)
            {
                var parts = Line.Trim().Split(); // 
                string operation = parts[0].ToUpper();
                string[] operands = ["", ""];

                if (parts.Length == 3)
                {
                    operands[1] = parts[2];
                    operands[0] = parts[1].Substring(0, parts[1].Length - 1);
                }

                //тут порушується принцип  open-close. Для виправленння коду потрібно використати шаблон "стратегія"
                if (operation == "MOV")
                    return new MOV(operands[0], operands[1]);
                if (operation == "ADD")
                    return new ADD(operands[0], operands[1]);
                throw new Exception($"unknown operation {operation}");
            }
        }


        //======= фабрика ===//
        // фабрика вміє створювати список команд із тексту програми та повертати машину для виконання
        interface IMachineFactory
        {
            List<IExecutable> CreateProgram(string ProgramText);
            Machine GetMachine();

        }

        class MachineFactory : IMachineFactory
        {
            protected IOperatorFactoryMethod factoryMethod;
            public List<IExecutable> CreateProgram(string ProgramText)
            {
                var program = new List<IExecutable>();

                foreach (string line in ProgramText.Split("\n"))
                {
                    //використовуємо фабричний метод для створення кожного оператора із кожного рядка програми
                    program.Add(factoryMethod.CreateOperator(line));
                }
                return program;
            }

            public virtual Machine GetMachine()
            {
                return null;
            }
        }

        //фабрика для mix машини
        class MixMachineFactory : MachineFactory
        {
            public MixMachineFactory() : base()
            {
                factoryMethod = new MixOperatorFactoryMethod();
            }

            public override Machine GetMachine()
            {
                return MixMachine.getInstance();
            }
        }

        //фабрика для асемблер  машини
        class AsmMachineFactory : MachineFactory
        {
            public AsmMachineFactory() : base()
            {
                factoryMethod = new AsmOperatorFactoryMethod();
            }

            public override Machine GetMachine()
            {
                return AsmMachine.getInstance();
            }
        }
    }

}

