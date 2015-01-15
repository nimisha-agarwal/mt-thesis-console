using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace ConsoleApplication2
{
    public class LeftFactoring
    {
        public Hashtable newhtable = new Hashtable();

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                               FUNCTION TO SORT VALUES PART OF HASHTABLE
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>

        String[][] SORTING(String[][] part)
        {
            String[] data = new String[part.Length];
            for (int i = 0; i < part.Length; i++)
                data[i] = String.Join(" ", part[i]);
            Array.Sort(data);
            String[][] part1 = new String[part.Length][];
            for (int i = 0; i < part1.Length; i++)
                part1[i] = Regex.Split(data[i].Trim(), " ");
            int max = part1[0].Length;
            for (int i = 0; i < part1.Length; i++)
            {
                if (part1[i].Length > max)
                    max = part1[i].Length;
            }
            String[][] values = new String[part1.Length][];
            for (int i = 0; i < part1.Length; i++)
            {
                int j;
                values[i] = new String[max];
                for (j = 0; j < part1[i].Length; j++)
                    values[i][j] = part1[i][j];
                if (j < max)
                {
                    for (int k = j; k < max; k++)
                        values[i][k] = " ";
                }
            }
           
            return values;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                     RECURSIVE FUNCTION TO FIND LEFT FACTORS FOR PARTICULAR ENTRY IN HASHTABLE
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
        /// </summary>
        /// <param name="values"></param>
        /// <param name="derivation"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="index"></param>

        void LLFACTOR(String[][] values, ref Hashtable derivation, int start, int stop, int index)
        {
            if (start == stop)
            {
                String[] prefix;
                String[] suffix;
                ArrayList arr = new ArrayList();
                if (index - 1 <= 0)
                {
                    prefix = new String[1];
                    prefix[0] = " ";
                    suffix = new String[1];
                    suffix[0] = values[start][0];
                }
                else
                {
                    prefix = new String[index - 1];
                    for (int i = 0; i < prefix.Length; i++)
                        prefix[i] = values[start][i];
                    suffix = new String[values[start].Length - (index - 1)];
                    for (int i = 0, j = index - 1; i < suffix.Length; j++, i++)
                    {
                        if (values[start][j] == " ")
                        {
                            suffix = new String[1];
                            suffix[0] = "epsilon";
                            break;
                        }
                        else
                            suffix[i] = values[start][j];
                    }
                }
                String strprefix = String.Join("", prefix);
                arr.Add(suffix);
                if (derivation.ContainsKey(strprefix))
                {
                    ArrayList list = (ArrayList)derivation[strprefix];
                    list.Add(suffix);
                    derivation[strprefix] = list;
                }
                else
                    derivation[strprefix] = arr;
                return;
            }

            int p = start, q = start;
            for (int i = start; i < stop; i++)
            {
                if (values[i][index] == values[i + 1][index])
                    q++;
                else
                {
                    LLFACTOR(values, ref derivation, p, q, index + 1);
                    p = q + 1;
                    q++;
                    i = q - 1;
                }
            }
            LLFACTOR(values, ref derivation, p, q, index + 1);
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                           FUNCTION TO LEFT FACTOR THE GRAMMAR
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="htable"></param>
        /// <param name="newhtable"></param>

        public Hashtable LLFACTORING(Hashtable oldhtable)
        {      
            foreach (DictionaryEntry entry in oldhtable)
            {
                String[][] part = (String[][])oldhtable[entry.Key];
                String[][] values = SORTING(part);
                Hashtable derivation = new Hashtable();
                LLFACTOR(values, ref derivation, 0, values.Length - 1, 0);
                int count = 1;
                foreach (DictionaryEntry e in derivation)
                {
                    if ((String)e.Key == " ")
                    {
                        ArrayList list = (ArrayList)derivation[e.Key];
                        count = list.Count;
                    }
                }
                String[][] newpart = new String[derivation.Count + count - 1][];
                int i = 0;
                String s = entry.Key + "'";
                foreach (DictionaryEntry e in derivation)
                {
                    if ((String)e.Key != " ")
                    {
                        String[] temp = Regex.Split(((String)e.Key).Trim(), "");
                        String[] t = new String[temp.Length - 1];
                        int m, n;
                        for (m = 0, n = 1; m < t.Length - 1; m++, n++)
                            t[m] = temp[n];
                        t[m] = s;
                        newpart[i] = new String[t.Length];
                        for (int l = 0; l < t.Length; l++)
                            newpart[i][l] = t[l];
                        ArrayList list = (ArrayList)derivation[e.Key];
                        String[][] newlist = (String[][])list.ToArray(typeof(String[]));
                        String[][] newvalue = new String[newlist.Length][];
                        for (int ind = 0; ind < newlist.Length; ind++)
                        {
                            String[] v = newlist[ind];
                            newvalue[ind] = new String[v.Length];
                            for (int ind1 = 0; ind1 < newlist[ind].Length; ind1++)
                                newvalue[ind][ind1] = v[ind1];
                        }
                        newhtable.Add(s, newvalue);
                        s = s + "'";
                    }
                    else
                    {
                        ArrayList list = (ArrayList)derivation[e.Key];
                        String[][] newlist = (String[][])list.ToArray(typeof(String[]));
                        for (int ind = 0; ind < newlist.Length; ind++)
                        {
                            String[] v = newlist[ind];
                            newpart[i] = new String[v.Length];
                            for (int ind1 = 0; ind1 < newlist[ind].Length; ind1++, i++)
                                newpart[i][ind1] = v[ind1];
                        }
                        i--;
                    }
                    i++;
                }
                newhtable.Add(entry.Key, newpart);
            }             
            return newhtable;
        }
    }
}