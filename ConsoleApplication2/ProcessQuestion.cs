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

        String[] Question1(String sym, HashSet<String> terminals,int ch)
        {
            String[] ques1 = new string[2];

            if(ch==1)
                ques1[0] = "Which symbols should be included in FIRST[" + sym + "] ?";
            else if(ch==2)
                ques1[0] = "Which symbols should be included in FOLLOW[" + sym + "] ?";

            ques1[1] = string.Join(" ", terminals);
            return ques1;
        }

        String[] Question2(String sym, String terminalsymbol,int ch)
        {
            String[] ques2 = new string[2];
            if(ch==1)
            {
                String ques_subpart = "1. If X is terminal, then FIRST(X) is {X}.  2. If X is nonterminal and X -> a alpha is a production, then add a to FIRST(X). If X -> epsilon is a production, then add epsilon to FIRST(X).  3. If X -> Y1Y2....Yk is a production, then for all i such that all of Y1,....,Yi-1 are nonterminals and FIRST(Yj) contains epsilon for j = 1, 2, ...., i-1 (i.e Y1Y2....Yi-1 -> epsilon), add every non-epsilon symbol in FIRST(Yi) to FIRST(X). If epsilon is in FIRST(Yj) for all j = 1, 2, ....., k, then add epsilon to FIRST(X).  4. No valid rule for this symbol.";
                ques2[0] = "By which of the following rule, you have included " + terminalsymbol + " in First[" + sym + "] ?  " + ques_subpart;
            }
            else if(ch==2)
            {
                String ques_subpart = "1. $ is in FOLLOW(S), where S is the start symbol.  2. If there is a production A -> alpha B beta, beta != epsilon, then everything in FIRST(beta) but epsilon is in FOLLOW(B).  3. If there is a production A -> alpha B, or a production A -> alpha B beta where FIRST(beta) contains epsilon (i.e., beta -> epsilon), then everything in FOLLOW(A) is in FOLLOW(B).  4. No valid rule for this symbol.";
                ques2[0] = "By which of the following rule, you have included " + terminalsymbol + " in Follow[" + sym + "] ?  " + ques_subpart;
            }
            ques2[1] = "1 2 3 4";
            return ques2;
        }

        String[] Question3(String sym, String correctsymbol,int ch)
        {
            String[] ques3 = new string[2];
            if (ch == 1)
                ques3[0] = "Does " + correctsymbol + " should be included in FIRST[" + sym + "] ?";
            else if(ch==2)
                ques3[0]="Does "+correctsymbol+" should be included in FIRST["+sym+"] ?";

            ques3[1] = "Yes No";
            return ques3;
        }

        public void Ques_Gen(Hashtable htable,Hashtable first, ref Stack<string> symbols, HashSet<string> terminals, ref int state, string sym, ref string ans, ref Stack<string> correct, ref Stack<string> incorrect, ref string wrong, ref string right,int ch,string startsym)
        {
            if (state == 1)
                ques = Question1(sym, terminals,ch);
            else
            {
                if (state == 2)
                {
                    if (ans == null || ans == "4")
                    {
                        if (incorrect.Count != 0)
                        {
                            wrong = incorrect.Pop();
                            ques = Question2(sym, wrong,ch);
                        }
                        else
                        {
                            state = 4;
                            ans = null;
                        }
                    }
                    else
                    {
                        ques = Question2(sym, wrong,ch);
                    }
                }

                if (state == 4)
                {
                    string ruleno = null;
                    if (ans != null)
                    {
                        if (ch == 1)
                        {
                            if (!char.IsUpper(sym[0]))
                                ruleno = "1";
                            else
                            {
                                int flag = 0;
                                string[][] part = (string[][])htable[sym];
                                for (int i = 0; i < part.Length; i++)
                                {
                                    if (right == part[i][0])
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
                        else if(ch==2)
                        {
                            if (right=="$" && sym == startsym)
                                ruleno = "1";
                            else
                            {
                                int flag = 0;
                                foreach (DictionaryEntry entry in htable)
                                {
                                    String[][] part = (String[][])htable[entry.Key];
                                    int i;
                                    foreach (String[] element in part)
                                    {
                                        for (i = 0; i < element.Length; i++)
                                        {
                                            if (element[i] == sym)
                                            {
                                                if ((i + 1) != element.Length)
                                                {
                                                    if (!char.IsUpper(element[i + 1][0]))
                                                    {
                                                        if(right==element[i+1])
                                                        {
                                                            flag = 1;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if(((HashSet<string>)first[element[i+1]]).Contains(right))
                                                        {
                                                            flag = 1;
                                                            break;
                                                        }
                                                    }
                                                }                                                
                                            }
                                        }
                                        if (flag == 1)
                                            break;
                                    }
                                    if (flag == 1)
                                        break;
                                }
                                if (flag == 1)
                                    ruleno = "2";
                                else
                                    ruleno = "3";
                            }
                        }
                    }

                    if (ans == null || ans == ruleno)
                    {
                        if (correct.Count == 0)
                        {
                            state = 1;
                            ans = null;
                            ques = Question1(sym, terminals,ch);
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
                        ques = Question2(sym, right,ch);
                    }
                }

                if (state == 3)
                {
                    if (ans == "Y" || ans == "y")
                    {
                        state = 4;
                        ques = Question2(sym, right,ch);
                    }
                    else
                    {
                        ques = Question3(sym, right,ch);
                    }
                }
            }
        }

        public string[] Question(Hashtable htable, Hashtable first,Hashtable follow, ref Stack<string> symbols, HashSet<string> terminals, ref int state, ref string sym, ref string ans, ref Stack<string> incorrect, ref Stack<string> correct, ref string wrong, ref string right,int ch,string startsym)
        {
            //if(symbols.Count!=0)
            {
                if (state == 1)
                {
                    HashSet<string> tempinitfSet = new HashSet<string>();
                    
                    if (ch == 1)
                        tempinitfSet = (HashSet<string>)first[sym];
                    else if (ch == 2)
                        tempinitfSet = (HashSet<string>)follow[sym];

                    HashSet<string> initfSet = new HashSet<string>();

                    foreach (string strfst in tempinitfSet)
                    {
                        initfSet.Add(strfst);
                    }

                    if (ans != null)
                    {
                        string[] ansarray = Regex.Split(ans.Trim(), " ");

                        for (int i = 0; i < ansarray.Length; i++)
                        {
                            if (!initfSet.Contains(ansarray[i]))
                            {
                                incorrect.Push(ansarray[i]);
                            }
                            initfSet.Remove(ansarray[i]);
                        }

                        foreach (string s in initfSet)
                        {
                            correct.Push(s);
                        }

                        if (incorrect.Count == 0 && correct.Count == 0)
                        {
                            if (symbols.Count != 0)
                                sym = symbols.Pop();
                        }
                        else
                        {
                            state = 2;
                            ans = null;
                        }
                    }
                }

                Ques_Gen(htable,first, ref symbols, terminals, ref state, sym, ref ans, ref correct, ref incorrect, ref wrong, ref right,ch,startsym);
            }

            return ques;
        }
    }
}
