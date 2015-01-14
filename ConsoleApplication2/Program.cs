using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            Console.WriteLine();
            string[] grammar = File.ReadAllLines(@"C:\Users\NimishaAg\Documents\Visual Studio 2013\Projects\ConsoleApplication2\ConsoleApplication2\Grammars\" + args[0] + ".txt");

            Hashtable htable = new Hashtable();
            GrammarPreprocessing gram_p = new GrammarPreprocessing();
            htable = gram_p.PARTITION(grammar);
            gram_p.Print(htable);
            Console.WriteLine();

            HashSet<string> symbolset = new HashSet<string>();
            HashSet<string> terminals = new HashSet<string>();

            First fst = new First();
            Hashtable first = fst.FIRST(htable, ref symbolset, ref terminals);

            ////////////////////////////////////////////// STORING START SYMBOL //////////////////////////////////////////////////////

            String startsym = gram_p.STARTSYM(grammar);

            Follow flw = new Follow();
            Hashtable follow = new Hashtable();
            flw.R_FOLLOW(htable, first, ref follow, startsym);

            Stack<string> symbols = new Stack<string>();
            foreach (string s in symbolset)
                symbols.Push(s);

            int state = 1;
            string sym = symbols.Pop();
            string ans = null;
            Stack<string> correct = new Stack<string>();
            Stack<string> incorrect = new Stack<string>();
            string wrong = null, right = null;

            Console.WriteLine("Questions can be generated for : ");
            Console.WriteLine("1. FIRST Set");
            Console.WriteLine("2. FOLLOW Set");
            Console.WriteLine();
            Console.WriteLine("Enter your choice");
            int ch = int.Parse(Console.ReadLine());

            QuestionSet QS = new QuestionSet();

            if (ch == 1)
            {
                QS.QuestionSetGeneration(htable, first, follow, symbols, terminals, state, sym, ans, incorrect, correct, wrong, right, ch, startsym);
            }
            else if (ch == 2)
            {
                terminals.Add("$");
                QS.QuestionSetGeneration(htable, first, follow, symbols, terminals, state, sym, ans, incorrect, correct, wrong, right, ch, startsym);
            }
        }
    }
}
