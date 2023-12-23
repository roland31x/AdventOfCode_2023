using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.ComponentModel.Design;
using System.Threading.Channels;

namespace D23
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 23;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "fdsafdsgffdshtrdsyhrdf";
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

                Thread.Sleep(6000);

                string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                Console.WriteLine(resp2);
            }
        }

        private static string Part1(List<string> lines)
        {
            SlopeMap m = new SlopeMap(lines);
            return m.LongestRouteSlopes().ToString();
        }
        public class SlopeMap
        {
            public List<string> m;
            int StartJ = -1;
            int EndJ = -1;
            int[,] marked;
            int[,] walls;
            int[,] slopes;
            static List<int[]> dirs = new List<int[]>() { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { -1, 0 }, new int[] { 0, -1 } };
            public SlopeMap(List<string> lines) 
            {
                m = lines;
                marked = new int[lines.Count, lines[0].Length];
                walls = new int[lines.Count, lines[0].Length];
                slopes = new int[lines.Count, lines[0].Length];

                for (int j = 0; j < m[0].Length; j++)
                {
                    if (m[0][j] == '.')
                        StartJ = j;
                    if (m.Last()[j] == '.')
                        EndJ = j;
                }
                for(int i = 0; i < m.Count; i++)
                {
                    for(int j = 0; j < m[i].Length; j++)
                    {
                        if (m[i][j] == '#')
                            walls[i, j] = 1;
                        if (m[i][j] == '>' || m[i][j] == 'v' || m[i][j] == '^' || m[i][j] == '<')
                        {
                            slopes[i, j] = GetDir(m[i][j]);
                        }
                    }
                }
            }
            public int LongestRouteSlopes()
            {
                int StartI = 0;
                int StartJ = this.StartJ;

                Queue<(int, int, int)> q = new Queue<(int, int, int)>();
                marked[StartI, StartJ] = 1;
                q.Enqueue((StartI, StartJ, -1));
                while (q.Any())
                {
                    (int deqi, int deqj, int lastdir) = q.Dequeue();
                    for(int d = 0; d < 4; d++)
                    {
                        int nexti = deqi + dirs[d][0];
                        int nextj = deqj + dirs[d][1];
                        if (nexti < 0 || nextj < 0 || nextj >= m[0].Length || nexti >= m.Count)
                            continue;
                        if (walls[nexti, nextj] == 1)
                            continue;
                        if (slopes[nexti, nextj] != 0) 
                            if(slopes[nexti, nextj] != d + 1)
                                continue;
                        if (lastdir != -1 && d == (lastdir + 2) % 4)
                            continue;
                        marked[nexti, nextj] = marked[deqi, deqj] + 1;
                        q.Enqueue((nexti, nextj, d));
                    }
                }

                return marked[m.Count - 1,EndJ] - 1;
            }
            public int LongestRouteOverall()
            {
                List<Node> nodes = new List<Node>();
                Node start = new Node(0, StartJ);
                Node end = new Node(m.Count - 1, EndJ);
                nodes.Add(start);
                nodes.Add(end);

                for (int i = 0; i < m.Count; i++)
                {
                    for (int j = 0; j < m[0].Length; j++)
                    {
                        if (m[i][j] == '.')
                        {
                            int ct = 0;
                            for (int d = 0; d < 4; d++)
                            {
                                int nexti = i + dirs[d][0];
                                int nextj = j + dirs[d][1];
                                if (nexti < 0 || nextj < 0 || nextj >= m[0].Length || nexti >= m.Count)
                                    continue;
                                if (walls[nexti, nextj] == 1)
                                    continue;
                                ct++;
                            }
                            if (ct > 2)
                            {
                                nodes.Add(new Node(i, j));
                            }
                        }
                        
                    }                   
                }

                foreach(Node n in nodes)
                {
                    Queue<(int,int)> q = new Queue<(int, int)>();
                    marked = new int[m.Count, m[0].Length];
                    marked[n.I,n.J] = 1;
                    q.Enqueue((n.I, n.J));
                    while (q.Any())
                    {
                        (int deqi, int deqj) = q.Dequeue();
                        bool found = false;
                        foreach(Node neigh in nodes.Where(x => x != n))
                        {
                            if(deqi == neigh.I && deqj == neigh.J)
                            {
                                found = true;
                                n.Neighbors.Add(neigh);
                                n.dist.Add(marked[deqi, deqj] - 1);
                            }
                        }
                        if (found)
                        {
                            continue;
                        }
                        for (int d = 0; d < 4; d++)
                        {
                            int nexti = deqi + dirs[d][0];
                            int nextj = deqj + dirs[d][1];
                            if (nexti < 0 || nextj < 0 || nextj >= m[0].Length || nexti >= m.Count)
                                continue;
                            if (walls[nexti, nextj] == 1)
                                continue;
                            if (marked[nexti, nextj] != 0)
                                continue;
                            marked[nexti, nextj] = marked[deqi, deqj] + 1;
                            q.Enqueue((nexti, nextj));
                        }
                    }
                }
                marked = new int[m.Count, m[0].Length];

                Stack<Node> path = new Stack<Node>();
                Dictionary<Node, int> cache = new Dictionary<Node, int>();

                path.Push(start);
                int res = 0;
                DFS(path, 0, ref res);

                return res;
            }
            public void DFS(Stack<Node> path, int currentdist, ref int bestdist)
            {
                Node deq = path.Peek();
                if (deq.I == m.Count - 1 && deq.J == EndJ)
                {
                    if (bestdist < currentdist)
                        bestdist = currentdist;
                }
                else
                {                  
                    foreach(Node neighbor in deq.Neighbors)
                    {
                        if (path.Contains(neighbor))
                            continue;

                        path.Push(neighbor);
                        DFS(path, currentdist + deq.dist[deq.Neighbors.IndexOf(neighbor)], ref bestdist);
                        path.Pop();
                    }
                }
            }
            public int GetDir(char c)
            {
                return c switch
                {
                    'v' => 1,
                    '>' => 2,
                    '^' => 3,
                    '<' => 4,
                    _ => throw new Exception()
                };
            }
        }
        public class Node
        {
            public int I;
            public int J;
            public List<Node> Neighbors = new List<Node>();
            public List<int> dist = new List<int>();
            public Node(int i, int j)
            {
                this.I = i;
                this.J = j;
            }
            public override string ToString()
            {
                return $"({I},{J})";
            }
        }

        private static string Part2(List<string> lines)
        {
            SlopeMap m = new SlopeMap(lines);
            return m.LongestRouteOverall().ToString();
        }
    }
}
