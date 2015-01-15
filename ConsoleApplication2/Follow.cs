using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace ConsoleApplication2
{
    public class Follow
    {
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                     FOLLOW FUNCTION
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        void FOLLOW(ref Hashtable follow, Hashtable htable, String startsym, Hashtable first)
        {
            foreach (DictionaryEntry entry1 in htable)
            {
                HashSet<String> set = new HashSet<String>();
                String symbol = (String)entry1.Key;

                if (symbol.Trim() == startsym.Trim())
                    set.Add("$");
                foreach (DictionaryEntry entry in htable)
                {
                    String[][] part = (String[][])htable[entry.Key];
                    int i;
                    foreach (String[] element in part)
                    {
                        for (i = 0; i < element.Length; i++)
                        {
                            if (element[i] == symbol)
                            {
                                if ((i + 1) != element.Length)
                                {
                                    if (!char.IsUpper(element[i + 1][0]))
                                        set.Add(element[i + 1]);
                                    else
                                    {
                                        foreach (DictionaryEntry e in first)
                                        {
                                            if (element[i + 1] == (String)e.Key)
                                            {
                                                HashSet<String> val = (HashSet<String>)e.Value;

                                                foreach (String sym in val)
                                                {
                                                    if (sym != "epsilon")
                                                        set.Add(sym);
                                                    else
                                                    {
                                                        if (follow.ContainsKey(element[i + 1]))
                                                        {
                                                            HashSet<String> Initfollow = (HashSet<String>)follow[element[i + 1]];
                                                            String[] newInitfollow = Initfollow.ToArray();
                                                            foreach (String s in newInitfollow)
                                                                set.Add(s);
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (element[i] != (String)entry.Key)
                                    {
                                        if (follow.ContainsKey(entry.Key))
                                        {
                                            HashSet<String> Initfollow = (HashSet<String>)follow[entry.Key];
                                            String[] newInitfollow = Initfollow.ToArray();
                                            foreach (String s in newInitfollow)
                                                set.Add(s);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (follow.ContainsKey(entry1.Key))
                {
                    HashSet<String> newset = (HashSet<String>)follow[entry1.Key];
                    String[] temp = set.ToArray();
                    foreach (String s in temp)
                        newset.Add(s);
                    follow[entry1.Key] = newset;
                }
                else
                    follow[entry1.Key] = set;
            }
        }

        Hashtable clonefollow = new Hashtable();

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                               RECURSIVE FOLLOW FUNCTION
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        public void R_FOLLOW(Hashtable htable, Hashtable first, ref Hashtable follow, String startsym)
        {
            int flag = 0;
            //PARTITION(grammar);
            if (follow.Count != 0)
            {
                if (clonefollow.Count == follow.Count)
                {
                    foreach (DictionaryEntry e in follow)
                    {
                        HashSet<String> Initfollow = (HashSet<String>)follow[e.Key];
                        HashSet<String> Initclonefollow = (HashSet<String>)clonefollow[e.Key];
                        if (Initfollow != Initclonefollow)
                        {
                            flag = 1;
                            break;
                        }
                    }
                }
                else
                {
                    flag = 1;
                    FOLLOW(ref follow, htable, startsym, first);
                }
            }
            else
            {
                flag = 1;
                FOLLOW(ref follow, htable, startsym, first);
            }

            if (flag == 1)
            {
                foreach (DictionaryEntry e in follow)
                {
                    HashSet<String> set = (HashSet<String>)follow[e.Key];
                    clonefollow.Add(e.Key, set);
                }
                FOLLOW(ref follow, htable, startsym, first);
            }
            else
            {
                return;
            }
            R_FOLLOW(htable, first, ref follow, startsym);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                     PRINTING FOLLOW SET
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="follow"></param>

        public void PRINTFOLLOW(Hashtable follow)
        {
            foreach (DictionaryEntry e in follow)
            {
                Console.Write("FOLLOW[ " + e.Key + " ] = {");
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