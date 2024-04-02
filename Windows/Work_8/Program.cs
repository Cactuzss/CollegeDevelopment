using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work_8
{
    internal class Program
    {

        /*          3 Класса эквивалентности 
         * Input 1 1 1 Output Actute EXPECTED Right 
         *
         * Input 0 1 1 Output None
         * Input 1 0 1 Output None
         * Input 1 1 0 Output None
         * 
         * Input 5 5 7 Output Actute
         * Input 7 24 25 Output Actute
         * Input 13 5 12 Output RIGHT EXPECTED Obtuse
         * 
         * Input 100 1 1 Output None 
         * Input 2147483647 2147483647 2147483647 Output None EXPECTED Right
         * Input -1 -1 -1 Output None
         *
         */

        static void Main(string[] args)
        {
            int a;
            int b;
            int c;
            string res;
            Console.Write("Введите a: ");
            a = int.Parse(Console.ReadLine());
            Console.Write("Введите b: ");
            b = int.Parse(Console.ReadLine());
            Console.Write("Введите c: ");
            c = int.Parse(Console.ReadLine());
            int aa = a * a;
            int bb = b * b;
            int cc = c * c;
            if (a >= b + c || b >= a + c || c >= a + b)
            {
                res = "NONE";
            }
            else
            if (aa == bb + cc || bb == aa + cc || cc == aa + bb)
            {
                res = "RIGHT";
            }
            else
            if (aa < bb + cc && bb < aa + cc && cc < aa + bb)
            {
                res = "ACUTE";
            }
            else
            {
                res = "OBTUSE";
            }
            Console.WriteLine(res);
            Console.ReadLine();
        }

    }
}
