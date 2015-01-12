using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace ConsoleApplication2
{
    class FirstQuestionSet
    {
        string previous_question = null;
        public void QuestionSetGeneration(Hashtable htable, Hashtable first, Stack<string> symbols, HashSet<string> terminals, int state, string sym, string ans, Stack<string> incorrect, Stack<string> correct, string wrong, string right)
        {
            ProcessQuestion processQ = new ProcessQuestion();
            string[] ques = processQ.Question(htable, first, ref symbols, terminals, ref state, ref sym, ref ans, ref incorrect, ref correct, ref wrong, ref right);

            if (state == 1 && ques[0] == previous_question)
            {
                Console.WriteLine("Success");
                return;
            }

            string[] question = Regex.Split(ques[0], "  ");

            for (int i = 0; i < question.Length; i++)
            {
                Console.WriteLine(question[i]);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            string[] answerchoices = Regex.Split(ques[1], " ");
            for (int i = 0; i < answerchoices.Length; i++)
            {
                Console.Write(answerchoices[i] + "          ");
            }

            Console.WriteLine();
            Console.WriteLine();
            ans = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine();

            previous_question = ques[0];

            QuestionSetGeneration(htable, first, symbols, terminals, state, sym, ans, incorrect, correct, wrong, right);
        }
    }
}
