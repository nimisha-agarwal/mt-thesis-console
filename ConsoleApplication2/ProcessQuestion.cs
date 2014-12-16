using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace ConsoleApplication2
{
    class ProcessQuestion
    {
        string[] ques = new string[2];
        string right;

        String[] Question1(String sym, HashSet<String> terminals)
        {
            String[] ques1 = new string[2];
            ques1[0] = "What is FIRST[" + sym + "] ?";
            ques1[1] = string.Join(" ", terminals);
            return ques1;
        }

        String[] Question2(String sym, String terminalsymbol)
        {
            String ques_subpart = "1. If X is terminal, then FIRST(X) is {X}.  2. If X is nonterminal and X -> a alpha is a production, then add a to FIRST(X). If X -> epsilon is a production, then add epsilon to FIRST(X).  3. If X -> Y1Y2....Yk is a production, then for all i such that all of Y1,....,Yi-1 are nonterminals and FIRST(Yj) contains epsilon for j = 1, 2, ...., i-1 (i.e Y1Y2....Yi-1 -> epsilon), add every non-epsilon symbol in FIRST(Yi) to FIRST(X). If epsilon is in FIRST(Yj) for all j = 1, 2, ....., k, then add epsilon to FIRST(X).  4. No valid rule for this symbol.";
            String[] ques2 = new string[2];
            ques2[0] = "By which of the following rule, you have included " + terminalsymbol + " in First[" + sym + "] ?  " + ques_subpart;
            ques2[1] = "1 2 3 4";
            return ques2;
        }

        String[] Question3(String sym, String correctsymbol)
        {
            String[] ques3 = new string[2];
            ques3[0] = "Does " + correctsymbol + " should be included in FIRST[" + sym + "] ?";
            ques3[1] = "Yes No";
            return ques3;
        }

        public void Ques_Gen(Hashtable htable, ref Stack<string> symbols,HashSet<string> terminals,ref int state, string sym,ref string ans,ref Stack<string> correct,ref Stack<string> incorrect,ref string wrong,ref string right)
        {
            if (state == 1)
                ques = Question1(sym, terminals);
            else
            {
                if(state==2)
                {
                    if (ans == null || ans == "4")
                    {
                        if(incorrect.Count!=0)
                        {
                            wrong = incorrect.Pop();
                            ques = Question2(sym, wrong);
                        }
                        else
                        {
                            state = 4;
                            ans = null;
                        }
                    }
                    else
                    {
                        ques = Question2(sym, wrong);
                    }
                }                

                if(state==4)
                {
                    string ruleno=null;
                    if(ans!=null)
                    {
                        if (!char.IsUpper(sym[0]))
                            ruleno = "1";
                        else
                        {
                            int flag = 0;
                            string[][] part = (string[][])htable[sym];
                            for(int i=0;i<part.Length;i++)
                            {
                                if(right==part[i][0])
                                {
                                    flag = 1;
                                    break;
                                }
                            }
                            if (flag == 1)
                                ruleno = "2";
                            else
                                ruleno = "3";
                        }
                    }

                    if(ans==null || ans ==ruleno)
                    {
                        if(correct.Count==0)
                        {
                            state = 1;
                            ans = null;
                            ques = Question1(sym, terminals);
                        }
                        else
                        {
                            right = correct.Pop();
                            state = 3;
                            //ques = Question3(sym, right);
                        }
                    }
                    else
                    {
                        ques = Question2(sym, right);
                    }
                }
                
                if(state==3)
                {
                    if(ans=="Y" || ans=="y")
                    {
                        state = 4;
                        ques = Question2(sym, right);
                    }
                    else
                    {
                        ques = Question3(sym, right);
                    }
                }
            }
        }

        public string[] Question(Hashtable htable, Hashtable first, ref Stack<string> symbols, HashSet<string> terminals, ref int state, ref string sym,ref string ans,ref Stack<string> incorrect,ref Stack<string> correct,ref string wrong,ref string right)
        {
            if(symbols.Count!=0)
            {
                if(state==1)
                {
                    HashSet<string> tempsymfirst = (HashSet<string>)first[sym];
                    HashSet<string> symfirst = new HashSet<string>();

                    foreach(string sfirst in tempsymfirst)
                    {
                        symfirst.Add(sfirst);
                    }

                    if (ans != null)
                    {
                        string[] ansarray = Regex.Split(ans.Trim(), " ");

                        for (int i = 0; i < ansarray.Length; i++)
                        {
                            if (!symfirst.Contains(ansarray[i]))
                            {
                                incorrect.Push(ansarray[i]);
                            }
                            symfirst.Remove(ansarray[i]);
                        }

                        foreach (string s in symfirst)
                        {
                            correct.Push(s);
                        }

                        if (incorrect.Count == 0 && correct.Count == 0)
                        {
                            sym = symbols.Pop();
                        }
                        else
                        {
                            state = 2;
                            ans = null;
                        }
                    }
                }

                Ques_Gen(htable, ref symbols, terminals, ref state, sym, ref ans, ref correct, ref incorrect,ref wrong,ref right);
            }

            return ques;
        }
    }
}
