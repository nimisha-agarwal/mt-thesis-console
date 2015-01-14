using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ConsoleApplication2
{
    public class SymbolsDivision
    {
        //public HashSet<String> terminals = new HashSet<string>();
        //public HashSet<String> nonterminals = new HashSet<string>();
        //HashSet<String> grammarsymbols = new HashSet<string>();

        public SymbolsDivision(Hashtable htable, ref HashSet<string> symbolset, ref HashSet<string> terminals)
        {
            foreach (DictionaryEntry entry in htable)
            {
                //nonterminals.Add((String)entry.Key);
                String[][] part = (String[][])htable[entry.Key];
                foreach (String[] element in part)
                {
                    for (int i = 0; i < element.Length; i++)
                    {
                        if (!char.IsUpper(element[i][0]))
                            terminals.Add(element[i]);

                        else
                            symbolset.Add(element[i]);
                    }
                }
            }

        }
    }
}