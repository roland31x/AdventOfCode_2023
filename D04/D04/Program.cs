using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D04
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 4;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "coooooookie";
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

        private static string Part1(List<string> lines)
        {
            int toreturn = 0;

            for(int i = 0; i < lines.Count; i++)
            {
                string cards = lines[i].Split(':')[1];
                string winning = cards.Split("|")[0].Trim();
                string[] win = winning.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string having = cards.Split("|")[1].Trim();
                string[] have = having.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> ok = win.Intersect(have).ToList();

                int score = 0;
                if (ok.Count == 1)
                    score = 1;
                else
                    score = (int)Math.Pow(2, ok.Count - 1);

                toreturn += score;
            }

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            int toreturn = 0;

            Dictionary<int, int> cardvals = new Dictionary<int, int>();

            for (int i = 0; i < lines.Count; i++)
                toreturn += 1 + GetScore(lines[i], cardvals, i + 1, lines);

            return toreturn.ToString();
        }
        public static int GetScore(string line, Dictionary<int,int> cardvals, int card, List<string> lines)
        {
            if (cardvals.ContainsKey(card))
            {
                return cardvals[card];
            }
                
            else
            {
                string cards = line.Split(':')[1];
                string winning = cards.Split("|")[0].Trim();
                string[] win = winning.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string having = cards.Split("|")[1].Trim();
                string[] have = having.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> ok = win.Intersect(have).ToList();

                int score = ok.Count;              

                for (int j = 1; j <= ok.Count; j++)
                    score += GetScore(lines[card + j - 1], cardvals, card + j, lines);

                cardvals[card] = score;

                return score;
            }
        }
    }
}
