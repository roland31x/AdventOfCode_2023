using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
namespace D09
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 9;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "coookiiiieeeee :3";
        public static void Main(string[] args)
        {
            List<string> lines = AOCIn.GetInput(InputPath, YEAR, DAY, SeshCookie);

            string part1 = Part1(lines);
            Console.WriteLine("Part 1 solution: " + part1);

            string part2 = Part2(lines);
            Console.WriteLine("Part 2 solution: " + part2);

            Console.WriteLine("Submit? Y/N");
            string command = Console.ReadLine();
            if (command.ToUpper() == "Y")
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

            foreach(string line in lines)
            {
                List<int> initial = new List<int>();
                string[] tok = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tok)
                    initial.Add(int.Parse(s));

                List<List<int>> results = new List<List<int>>();
                results.Add(initial);
                bool ready = false;
                while (!ready)
                {
                    List<int> nextgen = new List<int>();
                    HashSet<int> set = new HashSet<int>();
                    for(int i = 0; i < results.Last().Count - 1; i++)
                    {
                        int element = results.Last()[i + 1] - results.Last()[i];
                        nextgen.Add(element);
                        set.Add(element);
                    }
                    results.Add(nextgen);
                    if (set.Count == 1)
                        ready = true;
                }

                for(int i = results.Count - 1; i >= 1; i--)
                    results[i - 1].Add(results[i].Last() + results[i - 1].Last());

                toreturn += results[0].Last();
            }

            return toreturn.ToString();
        }
        static string Part2(List<string> lines)
        {
            int toreturn = 0;

            foreach (string line in lines)
            {
                List<int> initial = new List<int>();
                string[] tok = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tok)
                    initial.Add(int.Parse(s));

                List<List<int>> results = new List<List<int>>();
                results.Add(initial);
                bool ready = false;
                while (!ready)
                {
                    List<int> nextgen = new List<int>();
                    HashSet<int> set = new HashSet<int>();
                    for (int i = 0; i < results.Last().Count - 1; i++)
                    {
                        int element = results.Last()[i + 1] - results.Last()[i];
                        nextgen.Add(element);
                        set.Add(element);
                    }
                    results.Add(nextgen);
                    if (set.Count == 1)
                        ready = true;
                }

                for (int i = results.Count - 1; i >= 1; i--)
                    results[i - 1].Insert(0, results[i - 1].First() - results[i].First());

                toreturn += results[0].First();
            }

            return toreturn.ToString();
        }
    }
}
