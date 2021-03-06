﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;

namespace ConsoleApplication2
{
    public class First
    {
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                 RECURSIVE FIRST FUNCTION
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        public HashSet<String> R_FIRST(String symbol, Hashtable htable)
        {
            HashSet<String> set = new HashSet<String>();
            if (!char.IsUpper(symbol[0]))
                set.Add(symbol);
            else
            {
                String[][] part = (String[][])htable[symbol];
                String sym = "";
                foreach (String[] element in part)
                {
                    /*for (int i = 0; i < element.Length; i++ )
                        Console.WriteLine(element[i]);*/
                    if (element.Length != 1)
                        sym = element[1];
                    if (element[0] == symbol)
                        continue;
                    set.UnionWith(R_FIRST(element[0], htable));
                    if (set.Contains("epsilon") && sym != "" && !char.IsUpper(sym[0]))
                        set.Add(sym);
                }
            }

            return set;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                  FIRST FUNCTION
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="grammar"></param>

        public Hashtable FIRST(Hashtable htable)
        {
            Hashtable first = new Hashtable();
            //SymbolsDivision sym = new SymbolsDivision(htable,ref symbolset,ref terminals);
            
            foreach (DictionaryEntry entry in htable)
            {
                HashSet<String> Initfirst = new HashSet<String>();
                //Console.WriteLine(entry.Key);
                Initfirst = R_FIRST((String)entry.Key, htable);
                first.Add(entry.Key, Initfirst);
            }           
            
            return first;     
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                   PRINTING FIRST SET
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public void PRINTFIRST(Hashtable first)
        {
            foreach (DictionaryEntry e in first)
            {
                Console.Write("FIRST[ " + e.Key + " ] = {");
                HashSet<String> val = (HashSet<String>)e.Value;

                foreach (String i in val)
                {
                    Console.Write(" " + i + ",");
                }
                Console.Write("\b");
                Console.WriteLine(" }");
            }
        }
    }
}