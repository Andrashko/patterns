﻿using System;
using System.Text;
using Test;

namespace Patterns
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StructuralPatterns.TestMonad();
            Console.ReadLine();
        }
    }
}
