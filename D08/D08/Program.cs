using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D08
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 8;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cooking";
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
            long toreturn = 0;

            string instruction = lines[0];
            
            for(int i = 2; i < lines.Count; i++)
            {
                Node parent = Node.GetNode(lines[i].Split('=')[0].Trim());
                Node Left = Node.GetNode(lines[i].Split('=')[1].Trim().Split(',')[0].Trim().Split('(')[1].Trim());
                Node Right = Node.GetNode(lines[i].Split('=')[1].Trim().Split(',')[1].Trim().Split(')')[0].Trim());
                parent.LR[0] = Left;
                parent.LR[1] = Right;
            }
            Node current = Node.GetNode("AAA");
            int steps = 0;
            while(current.Name != "ZZZ")
            {
                char com = instruction[steps % instruction.Length];
                if (com == 'L')
                    current = current.LR[0];
                else
                    current = current.LR[1];
                steps++;
            }

            toreturn = steps;

            return toreturn.ToString();
        }
        public class Node
        {
            public static List<Node> nodes = new List<Node>();
            public Node[] LR = new Node[2];
            public string Name;
            public Node(string name)
            {
                Name = name;
            }
            public static Node GetNode(string name)
            {
                if(nodes.Any(n => n.Name == name))
                    return nodes.First(n => n.Name == name);
                else
                {
                    Node tor = new Node(name);
                    nodes.Add(tor);
                    return tor;
                }
            }
            public override string ToString()
            {
                return Name;
            }
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            string instruction = lines[0];

            for (int i = 2; i < lines.Count; i++)
            {
                Node parent = Node.GetNode(lines[i].Split('=')[0].Trim());
                Node Left = Node.GetNode(lines[i].Split('=')[1].Trim().Split(',')[0].Trim().Split('(')[1].Trim());
                Node Right = Node.GetNode(lines[i].Split('=')[1].Trim().Split(',')[1].Trim().Split(')')[0].Trim());
                parent.LR[0] = Left;
                parent.LR[1] = Right;
            }

            List<Node> currents = Node.nodes.Where(x => x.Name[2] == 'A').ToList();
            List<int> result = new List<int>();
            
            for (int i = 0; i < currents.Count; i++)
            {
                int steps = 0;
                while (currents[i].Name[2] != 'Z')
                {
                    char com = instruction[steps % instruction.Length];
                    if (com == 'L')
                        currents[i] = currents[i].LR[0];
                    else
                        currents[i] = currents[i].LR[1];
                    steps++;
                }
                result.Add(steps);
            }

            toreturn = lcm(result[0], result[1]);
            for(int i = 2; i < result.Count; i++)
                toreturn = lcm(toreturn, result[i]);
            

            return toreturn.ToString();

        }
        static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
