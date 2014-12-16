using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

namespace ConsoleApplication2
{
    public class GrammarPreprocessing
    {
        public Hashtable oldhtable = new Hashtable();

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 CREATION OF HASHTABLE
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="grammar"></param>

        public Hashtable PARTITION(String[] grammar)
        {
            for (int g_index = 0; g_index < grammar.Length; g_index++)
            {
                String[] parts = Regex.Split(grammar[g_index].Trim(), "->");
                //Console.WriteLine(parts.Length);
                String[] partition = Regex.Split(parts[1].Trim(), "\\|");
                //Console.WriteLine(partition.Length);
                String[][] set = new String[partition.Length][];
                String str = null;
                for (int p_index = 0; p_index < partition.Length; p_index++)
                {
                    str += partition[p_index].Trim() + "  ";
                    String[] symb = Regex.Split(partition[p_index].Trim(), " ");
                    set[p_index] = symb;
                }

                oldhtable.Add(parts[0].Trim(), set);
            }
            return oldhtable;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                            FUNCTION TO PRINT HASHTABLE
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="htable"></param>

        public void Print(Hashtable htable)
        {
            foreach (DictionaryEntry e in htable)
            {
                Console.Write(e.Key + " ->");
                String[][] values = (String[][])e.Value;
                for (int i = 0; i < values.Length; i++)
                {
                    Console.Write(" ");
                    String[] data = (String[])values[i];
                    for (int j = 0; j < data.Length; j++)
                        Console.Write(data[j]);
                    Console.Write(" |");
                }
                Console.Write("\b");
                Console.WriteLine(" ");
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 STORING START SYMBOL
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="grammar"></param>
        /// <returns></returns>

        public String STARTSYM(String[] grammar)
        {
            String startsym;
            char[] st = new char[grammar[0].Length];
            int l;
            for (l = 0; l < grammar[0].Length; l++)
            {
                if (grammar[0][l] == ' ' && grammar[0][l + 1] == '-' && grammar[0][l + 2] == '>')
                    break;
                st[l] = grammar[0][l];
            }
            char[] start = new char[l];
            for (int m = 0; m < start.Length; m++)
                start[m] = st[m];
            startsym = new String(start);
            return startsym;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 FUNCTION TO CHECK LEFT RECURSION
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public bool CHECKLEFTRECURSION(Hashtable htable)
        {
            foreach (DictionaryEntry entry in htable)
            {
                String[][] part = (String[][])htable[entry.Key];
                foreach (String[] element in part)
                {
                    if (element[0] == (String)entry.Key)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 FUNCTION TO CHECK FOR LEFT FACTORING
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public bool CHECKLEFTFACTORING(Hashtable htable)
        {
            foreach (DictionaryEntry entry in htable)
            {
                String[][] part = (String[][])htable[entry.Key];
                for (int i = 0; i < part.Length; i++)
                {
                    for (int j = i + 1; j < part.Length; j++)
                    {
                        if (part[i][0] == part[j][0])
                            return true;
                    }
                }
            }
            return false;
        }

    }
}