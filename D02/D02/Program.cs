using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D02
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 2;
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
            for(int i = 0; i < lines.Count; i++)
            {
                bool ok = true;
                int gameid = i + 1;

                string info = lines[i].Split(':')[1].Trim();
                string[] subsets = info.Split(';');

                foreach(string subset in subsets)
                {
                    string[] data = subset.Split(',');
                    foreach(string d in data)
                    {
                        string inf = d.Trim();
                        int nr = int.Parse(inf.Split(' ')[0]);
                        string color = inf.Split(' ')[1];

                        if (color == "red" && nr > 12)
                            ok = false;
                        if (color == "green" && nr > 13)
                            ok = false;
                        if (color == "blue" && nr > 14)
                            ok = false;
                    }
                }

                if (ok)
                    toreturn += gameid;
            }
            return toreturn.ToString();
        }
        static string Part2(List<string> lines)
        {
            int toreturn = 0;
           
            for (int i = 0; i < lines.Count; i++)
            {
                int local = 1;
                Dictionary<string, int> vals = new Dictionary<string, int>();

                string info = lines[i].Split(':')[1].Trim();
                string[] subsets = info.Split(';');

                foreach (string subset in subsets)
                {
                    string[] data = subset.Split(',');
                    foreach (string d in data)
                    {
                        string inf = d.Trim();
                        int nr = int.Parse(inf.Split(' ')[0]);
                        string color = inf.Split(' ')[1];
                        if(vals.ContainsKey(color))
                            vals[color] = Math.Max(vals[color], nr);
                        else vals.Add(color, nr);
                    }
                }

                foreach (int value in vals.Values)
                    local *= value;
                toreturn += local;
            }

            return toreturn.ToString();
        }
    }
}