using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusRelayService
{
    class CalculatorImplementation : ICalculator
    {
        public int Add(int a, int b)
        {
            Console.WriteLine($"Calculating a:{a} + b:{b} = {a+b}");
            return a + b;
        }
    }
}
