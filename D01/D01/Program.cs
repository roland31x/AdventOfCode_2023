using AOCApi;
using System.Collections.Generic;
using System.Net;
using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D01
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 1;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "YOUR COOKIE HERE";
        public static void Main(string[] args)
        {
            List<string> lines = AOCIn.GetInput(InputPath, YEAR, DAY, SeshCookie);

            string part1 = Part1(lines);
            Console.WriteLine("Part 1 solution: " + part1);          

            string part2 = Part2(lines);
            Console.WriteLine("Part 2 solution: " + part2);            

            Console.WriteLine("Submit? Y/N");
            string command = Console.ReadLine();
            if(command.ToUpper() == "Y")
            {
                string resp = AOCOut.Submit(part1, YEAR, DAY, 1, SeshCookie);
                Console.WriteLine(resp);
                Thread.Sleep(100);
                string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                Console.WriteLine(resp2);
            }
        }

        static string Part1(List<string> lines)
        {
            int toreturn = 0;
            for(int i = 0; i < lines.Count; i++)
            {
                List<int> ints = new List<int>();
                for(int j = 0; j < lines[i].Length; j++)
                    if (int.TryParse(lines[i][j].ToString(), out int conv))
                        ints.Add(conv);

                toreturn += 10 * ints.First() + ints.Last();
            }
            return toreturn.ToString();
        }
        static string Part2(List<string> lines)
        {
            int toreturn = 0;
            for(int i = 0; i < lines.Count; i++)
            {
                List<int> ints = new List<int>();
                for(int j = 0; j < lines[i].Length; j++)
                {
                    try
                    {
                        if (int.TryParse(lines[i][j].ToString(), out int conv))
                            ints.Add(conv);
                        else if (TryParseCharNumber(lines[i], j, out int parsed))
                            ints.Add(parsed);
                    }
                    catch (Exception)
                    {
                        continue;
                    }                     
                }
                toreturn += 10 * ints.First() + ints.Last();
            }
            return toreturn.ToString();
        }
        static bool TryParseCharNumber(string s, int j, out int parsed)
        {
            parsed = 0;
            if (s[j] == 'o' && s[j + 1] == 'n' && s[j + 2] == 'e')
                parsed = 1;
            if (s[j] == 't' && s[j + 1] == 'w' && s[j + 2] == 'o')
                parsed = 2;
            if (s[j] == 't' && s[j + 1] == 'h' && s[j + 2] == 'r' && s[j + 3] == 'e' && s[j + 4] == 'e')
                parsed = 3;
            if (s[j] == 'f' && s[j + 1] == 'o' && s[j + 2] == 'u' && s[j + 3] == 'r')
                parsed = 4;
            if (s[j] == 'f' && s[j + 1] == 'i' && s[j + 2] == 'v' && s[j + 3] == 'e')
                parsed = 5;
            if (s[j] == 's' && s[j + 1] == 'i' && s[j + 2] == 'x')
                parsed = 6;
            if (s[j] == 's' && s[j + 1] == 'e' && s[j + 2] == 'v' && s[j + 3] == 'e' && s[j + 4] == 'n')
                parsed = 7;
            if (s[j] == 'e' && s[j + 1] == 'i' && s[j + 2] == 'g' && s[j + 3] == 'h' && s[j + 4] == 't')
                parsed = 8;
            if (s[j] == 'n' && s[j + 1] == 'i' && s[j + 2] == 'n' && s[j + 3] == 'e')
                parsed = 9;

            if (parsed != 0)
                return true;
            return false;
        }       
    }    
}