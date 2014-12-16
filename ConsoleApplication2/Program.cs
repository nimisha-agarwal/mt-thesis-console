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
            GrammarPreprocessing p = new GrammarPreprocessing();
            htable = p.PARTITION(grammar);
            p.Print(htable);
            Console.WriteLine();

            HashSet<string> symbolset = new HashSet<string>();
            HashSet<string> terminals = new HashSet<string>();

            First fst = new First();
            Hashtable first = fst.FIRST(htable, ref symbolset, ref terminals);

            Stack<string> symbols = new Stack<string>();
            foreach (string s in symbolset)
                symbols.Push(s);

            int state = 1;
            string sym = symbols.Pop();
            string ans = null;
            Stack<string> correct = new Stack<string>();
            Stack<string> incorrect = new Stack<string>();
            string wrong = null, right = null;

            FirstQuestionSet QS = new FirstQuestionSet();
            QS.QuestionSetGeneration(htable, first, symbols, terminals, state, sym, ans, incorrect, correct, wrong, right);
        }
    }
}
