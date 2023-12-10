using System.Text.RegularExpressions;
using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;

namespace D10
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 10;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "this was bad";
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

            Map m = new Map(lines.Count, lines[0].Length);
            int starti = -1;
            int startj = -1;

            for(int i = 0; i < lines.Count; i++)
            {
                //| is a vertical pipe connecting north and south.
                //- is a horizontal pipe connecting east and west.
                //L is a 90 - degree bend connecting north and east.
                //J is a 90 - degree bend connecting north and west.
                //7 is a 90 - degree bend connecting south and west.
                //F is a 90 - degree bend connecting south and east.
                //. is ground; there is no pipe in this tile.
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '|')
                    {
                        if(i - 1 >= 0)
                            m[i, j].neighbors.Add(m[i - 1, j]);

                        if(i + 1 < lines.Count)
                            m[i, j].neighbors.Add(m[i + 1, j]);
                    }
                    if (lines[i][j] == '-')
                    {
                        if (j - 1 >= 0) 
                            m[i, j].neighbors.Add(m[i, j - 1]);

                        if(j + 1 < lines[i].Length)
                            m[i, j].neighbors.Add(m[i, j + 1]);
                    }
                    if (lines[i][j] == 'L')
                    {
                        if (i - 1 >= 0) 
                            m[i, j].neighbors.Add(m[i - 1, j]);

                        if (j + 1 < lines[i].Length)
                            m[i, j].neighbors.Add(m[i, j + 1]);
                    }
                    if (lines[i][j] == 'J')
                    {
                        if (i - 1 >= 0)
                            m[i, j].neighbors.Add(m[i - 1, j]);

                        if (j - 1 >= 0)
                            m[i, j].neighbors.Add(m[i, j - 1]);
                    }
                    if (lines[i][j] == '7')
                    {
                        if (j - 1 >= 0)
                            m[i, j].neighbors.Add(m[i, j - 1]);

                        if (i + 1 < lines.Count)
                            m[i, j].neighbors.Add(m[i + 1, j]);
                    }
                    if (lines[i][j] == 'F')
                    {
                        if (j + 1 < lines[i].Length)
                            m[i, j].neighbors.Add(m[i, j + 1]);

                        if (i + 1 < lines.Count)
                            m[i, j].neighbors.Add(m[i + 1, j]);
                    }
                    if (lines[i][j] == 'S')
                    {
                        m[i, j].Mark = 1;
                        starti = i;
                        startj = j;
                    }
                        
                }
            }

            List<Node> final = new List<Node>();
            for(int i = 0; i < m.n; i++)
                for(int j = 0; j < m.m; j++)
                    if (m[i, j].neighbors.Contains(m[starti,startj]))
                        m[starti, startj].neighbors.Add(m[i,j]);

            Queue<(int,int)> q = new Queue<(int, int)>();
            q.Enqueue((starti, startj));
            while (q.Any())
            {
                (int curri, int currj) = q.Dequeue();
                int mark = m[curri, currj].Mark;
                foreach(Node node in m[curri,currj].neighbors.Where(x => x.Mark == 0))
                {
                    m[node.I, node.J].Mark = mark + 1;
                    q.Enqueue((node.I, node.J));
                }
            }

            toreturn = m.Max() - 1;

            return toreturn.ToString();
        }
        public class Node
        {
            public List<Node> neighbors = new List<Node>();
            public int Mark = 0;
            public int I;
            public int J;
            public Node(int i, int j) 
            {
                I = i; J = j;
            }
        }
        public class Map
        {
            public Node[,] nodes;
            public int n;
            public int m;
            public Node this[int i, int j] => nodes[i,j];
            public static List<int[]> dirs4 = new List<int[]>() { new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, }; // Up, Right, Down, Left
            public Map(int n, int m) 
            {
                nodes = new Node[n, m];
                this.n = n;
                this.m = m;
                for(int i = 0; i < n; i++)
                    for(int j = 0; j < m; j++)
                        nodes[i,j] = new Node(i,j);
            }
            public int Max()
            {
                int maxmark = 0;
                for(int i = 0; i < n; i++)
                {
                    for(int j = 0; j < m; j++)
                    {
                        if (nodes[i,j].Mark > maxmark)
                            maxmark = nodes[i,j].Mark;
                    }
                }
                return maxmark;
            }
            public void SetNeighbor(Node node, int idx)
            {
                int i = node.I;
                int j = node.J;

                int seti = node.I + dirs4[idx][0] * 2;
                int setj = node.J + dirs4[idx][1] * 2;

                if (seti < 0 || seti >= n || setj < 0 || setj >= m)
                    return;

                int midi = i;
                int midj = j;
                if (idx == 0)
                    midi = seti + 1;
                else if (idx == 1)
                    midj = setj - 1;
                else if (idx == 2)
                    midi = seti - 1;
                else if (idx == 3)
                    midj = setj + 1;

                node.neighbors.Add(nodes[midi, midj]);
                nodes[midi, midj].neighbors.Add(nodes[seti, setj]);
            }
        }
        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            Map m = new Map(lines.Count * 2 + 1, lines[0].Length * 2 + 1);
            int starti = -1;
            int startj = -1;

            for (int i = 0; i < lines.Count; i++)
            {
                //| is a vertical pipe connecting north and south.
                //- is a horizontal pipe connecting east and west.
                //L is a 90 - degree bend connecting north and east.
                //J is a 90 - degree bend connecting north and west.
                //7 is a 90 - degree bend connecting south and west.
                //F is a 90 - degree bend connecting south and east.
                //. is ground; there is no pipe in this tile.
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '|')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 0);
                        m.SetNeighbor(m[i * 2, j * 2], 2);
                    }
                    if (lines[i][j] == '-')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 1);
                        m.SetNeighbor(m[i * 2, j * 2], 3);
                    }
                    if (lines[i][j] == 'L')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 0);
                        m.SetNeighbor(m[i * 2, j * 2], 1);
                    }
                    if (lines[i][j] == 'J')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 0);
                        m.SetNeighbor(m[i * 2, j * 2], 3);
                    }
                    if (lines[i][j] == '7')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 3);
                        m.SetNeighbor(m[i * 2, j * 2], 2);
                    }
                    if (lines[i][j] == 'F')
                    {
                        m.SetNeighbor(m[i * 2, j * 2], 1);
                        m.SetNeighbor(m[i * 2, j * 2], 2);
                    }
                    if (lines[i][j] == 'S')
                    {
                        m[i * 2, j * 2].Mark = 1;
                        starti = i * 2;
                        startj = j * 2;
                    }

                }
            }

            List<Node> final = new List<Node>();
            for (int i = 0; i < m.n; i++)
                for (int j = 0; j < m.m; j++)
                    if (m[i, j].neighbors.Contains(m[starti, startj]))
                    {
                        if (i < starti)
                            m.SetNeighbor(m[starti, startj], 0);
                        else if (i > starti)
                            m.SetNeighbor(m[starti, startj], 2);
                        else if (j < startj)
                            m.SetNeighbor(m[starti, startj], 3);
                        else
                            m.SetNeighbor(m[starti, startj], 1);
                    }
                        

            Queue<(int, int)> q = new Queue<(int, int)>();
            q.Enqueue((starti, startj));
            while (q.Any())
            {
                (int curri, int currj) = q.Dequeue();
                int mark = m[curri, currj].Mark;
                foreach (Node node in m[curri, currj].neighbors.Where(x => x.Mark == 0))
                {
                    m[node.I, node.J].Mark = mark + 1;
                    q.Enqueue((node.I, node.J));
                }
            }

            int max = m.Max();
            for (int i = 0; i < m.n; i++)
                for (int j = 0; j < m.m; j++)
                    if (m[i,j].Mark == max)
                    {
                        starti = i;
                        startj = j;
                    }

            int[,] marked = new int[m.n,m.m];
            int maxi = starti;
            int maxj = startj;
            marked[starti, startj] = 2;
            int currentmark = max;

            while(currentmark != 1)
            {
                foreach(Node n in m[starti, startj].neighbors)
                {
                    if(n.Mark == currentmark - 1 && marked[n.I, n.J] == 0)
                    {
                        starti = n.I;
                        startj = n.J;
                        marked[starti, startj] = 2;
                        currentmark--;
                                                 
                        break;
                    }
                }
            }

            starti = maxi;
            startj = maxj;
            currentmark = max;
            while (currentmark != 2)
            {
                foreach (Node n in m[starti, startj].neighbors)
                {
                    if (n.Mark == currentmark - 1 && marked[n.I, n.J] == 0)
                    {
                        starti = n.I;
                        startj = n.J;
                        marked[starti, startj] = 2;
                        currentmark--;
                        break;
                    }
                }
            }

            starti = 0;
            startj = 0;

            q.Clear();

            marked[starti, startj] = 1;
            q.Enqueue((starti, startj));
            while (q.Any())
            {
                (int deqi, int deqj) = q.Dequeue();

                if (deqi - 1 >= 0 && marked[deqi - 1, deqj] == 0)
                {
                    marked[deqi - 1, deqj] = 1;
                    q.Enqueue((deqi - 1, deqj));
                }
                if (deqj - 1 >= 0 && marked[deqi, deqj - 1] == 0)
                {
                    marked[deqi, deqj - 1] = 1;
                    q.Enqueue((deqi, deqj - 1));
                }
                if (deqi + 1 < m.n && marked[deqi + 1, deqj] == 0)
                {
                    marked[deqi + 1, deqj] = 1;
                    q.Enqueue((deqi + 1, deqj));
                }
                if (deqj + 1 < m.m && marked[deqi, deqj + 1] == 0)
                {
                    marked[deqi, deqj + 1] = 1;
                    q.Enqueue((deqi, deqj + 1));
                }
            }

            for (int i = 0; i < (m.n - 1) / 2; i++)
            {
                for (int j = 0; j < (m.m - 1) / 2; j++)
                {
                    if (marked[i * 2, j * 2] == 0)
                        toreturn++;
                }
            }

            return toreturn.ToString();
        }      
    }
}
