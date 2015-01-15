using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(args[0]);
            //Console.WriteLine();
            string[] grammar = File.ReadAllLines(@"C:\Users\NimishaAg\Documents\Visual Studio 2013\Projects\ConsoleApplication2\ConsoleApplication2\Grammars\" + args[0] + ".txt");

            Hashtable htable = new Hashtable();
            GrammarPreprocessing gram_p = new GrammarPreprocessing();
            htable = gram_p.PARTITION(grammar);
            gram_p.Print(htable);
            Console.WriteLine();

            HashSet<string> symbolset = new HashSet<string>();
            HashSet<string> terminals = new HashSet<string>();

            First fst = new First();
            Hashtable first = fst.FIRST(htable);

            ////////////////////////////////////////////// STORING START SYMBOL //////////////////////////////////////////////////////

            String startsym = gram_p.STARTSYM(grammar);

            Follow flw = new Follow();
            Hashtable follow = new Hashtable();
            flw.R_FOLLOW(htable, first, ref follow, startsym);

            SymbolsDivision symdiv = new SymbolsDivision(htable, ref symbolset, ref terminals);

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
            Console.WriteLine("3. LL Parsing");
            Console.WriteLine("4. SLR Parsing");
            Console.WriteLine("5. CLR Parsing");
            Console.WriteLine("6. LALR Parsing");
            Console.WriteLine();
            Console.WriteLine("Enter your choice");
            int ch = int.Parse(Console.ReadLine());
            Console.WriteLine();

            bool lrecursive;
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
            else if (ch == 3 || ch == 4 || ch == 5 || ch == 6)
            {
                lrecursive = gram_p.CHECKLEFTRECURSION(htable);
                bool lfactor = gram_p.CHECKLEFTFACTORING(htable);

                if (lfactor == true)
                {
                    Console.WriteLine("Grammar requires left factoring.");
                    Console.WriteLine("Do you want to left factor the grammar? Y or N");
                    String s = Console.ReadLine();
                    if (s == "y" || s == "Y")
                    {
                        htable.Clear();
                        LeftFactoring Lfactor = new LeftFactoring();
                        htable = Lfactor.LLFACTORING(gram_p.oldhtable);
                        //Print(htable);
                        Console.WriteLine();
                        Console.WriteLine("Grammar after Left Factoring");
                        Console.WriteLine();
                        gram_p.Print(htable);

                        first.Clear();
                        first = fst.FIRST(htable);

                        follow.Clear();
                        flw.R_FOLLOW(htable, first, ref follow, startsym);

                        symbolset.Clear();
                        terminals.Clear();
                        symdiv = new SymbolsDivision(htable, ref symbolset, ref terminals);

                        symbols.Clear();
                        foreach (string newstr in symbolset)
                            symbols.Push(newstr);
                    }
                    else
                        System.Environment.Exit(0);
                }

                Console.WriteLine();
                Console.WriteLine("FIRST Set");
                fst.PRINTFIRST(first);

                Console.WriteLine();
                Console.WriteLine("FOLLOW Set");
                flw.PRINTFOLLOW(follow);

                Console.ReadLine();

            }
        }
    }
}
