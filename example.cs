using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scratch
{
    class Program
    {
        static void Main(string[] args)
        {

        string exStr1 = "abc";
        string exStr2 = "def"; 

        joinedStrings strplusstr = ConcatStrings(exStr1, exStr2);

        Console.WriteLine("Str 1: {0} Str 2: {1} joined {2}", strplusstr.original1, strplusstr.original2, strplusstr.joinedString);

//                                      // main
        }

        class joinedStrings
        {
            public String original1;
            public String original2; 
            public String joinedString; 
             
        }

        private static joinedStrings ConcatStrings(string str1, string str2)
        {

            joinedStrings concatResult = new joinedStrings();

            concatResult.original1 = str1;

            concatResult.original2 = str2;

            concatResult.joinedString = str1 + str2;

            return concatResult;

        }
    }
}

use of static in declaration of function:

This is a common problem for newcomers to OO programming.

If you want to use a method of an object, you need to create an instance of it (using new). 
UNLESS, the method doesn't require the object itself, in which case it can (and should) be declared static.
