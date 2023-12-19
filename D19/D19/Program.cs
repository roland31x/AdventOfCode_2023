using AOCApi;
using System.Collections.Generic;
using System.Net;
using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D19
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 19;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "asdasdads";
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
            long toreturn = 0;

            int idx = 0;
            foreach (string line in lines)
            {
                idx++;
                if (line == string.Empty)
                    break;
                string name = line.Split('{')[0];
                string full = line.Split('{')[1].Replace("}", "");
                Workflow wf = Workflow.GetWorkflow(name);
                string[] ops = full.Split(',');
                for (int i = 0; i < ops.Length; i++)
                {
                    if(i == ops.Length - 1)
                    {
                        string IfTrue = ops[i];
                        wf.Operations.Add(new Ops(null, null, null, IfTrue));
                    }
                    else
                    {
                        char condition = ops[i][0];
                        char @operator = ops[i][1];
                        int value = int.Parse(ops[i].Split(@operator)[1].Split(':')[0]);
                        string IfTrue = ops[i].Split(@operator)[1].Split(':')[1];
                        wf.Operations.Add(new Ops(@operator,condition,value,IfTrue));
                    }
                }            
            }
            for (int i = idx; i < lines.Count; i++)
            {
                string line = lines[i];
                line = line.Replace("{", "").Replace("}", "");
                string[] tok = line.Split(',');
                int x = int.Parse(tok[0].Split('=')[1]);
                int m = int.Parse(tok[1].Split('=')[1]);
                int a = int.Parse(tok[2].Split('=')[1]);
                int s = int.Parse(tok[3].Split('=')[1]);

                Dictionary<char, int> dict = new Dictionary<char, int>();
                dict.Add('x', x);
                dict.Add('m', m);
                dict.Add('a', a);
                dict.Add('s', s);

                Workflow current = Workflow.GetWorkflow("in");
                string result = null;
                while(true)
                {
                    for(int o = 0; o < current.Operations.Count; o++)
                    {
                        if (o == current.Operations.Count - 1)
                            result = current.Operations[o].IfTrue!;
                        else
                        {
                            int valuetocompare = dict[(char)current.Operations[o].Condition!];
                            char comp = (char)current.Operations[o].Operator!;
                            int comparison = (int)current.Operations[o].Value!;
                            if((comp == '<' && valuetocompare < comparison) || (comp == '>' && valuetocompare > comparison))
                            {
                                result = current.Operations[o].IfTrue!;
                                break;
                            }
                        }
                    }
                    if(result == "A" || result == "R")
                    {
                        if(result == "A")
                        {
                            toreturn += x;
                            toreturn += a;
                            toreturn += m;
                            toreturn += s;
                        }
                        else
                        {
                            int ghhg = 0;
                        }
                        break;
                    }
                    current = Workflow.GetWorkflow(result);
                }

            }
            return toreturn.ToString();
        }
        public class Workflow
        {
            public static Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();
            public string Name { get; }

            public List<Ops> Operations = new List<Ops>();
            public Workflow(string name)
            {
                Name = name;
                workflows.Add(name, this);
            }
            public static Workflow GetWorkflow(string name)
            {
                if(workflows.TryGetValue(name, out Workflow wf)) 
                    return wf;
                return new Workflow(name);
            }
        }
        public class Ops
        {
            public char? Operator;
            public char? Condition;
            public int? Value;
            public string? IfTrue;

            public Ops(char? @operator, char? condition, int? value, string? ifTrue)
            {
                Operator = @operator;
                Condition = condition;
                IfTrue = ifTrue;
                Value = value;
            }
        }
        private static string Part2(List<string> lines)
        {
            Workflow.workflows.Clear();

            foreach (string line in lines)
            {
                if (line == string.Empty)
                    break;
                string name = line.Split('{')[0];
                string full = line.Split('{')[1].Replace("}", "");
                Workflow wf = Workflow.GetWorkflow(name);
                string[] ops = full.Split(',');
                for (int i = 0; i < ops.Length; i++)
                {
                    if (i == ops.Length - 1)
                    {
                        string IfTrue = ops[i];
                        wf.Operations.Add(new Ops(null, null, null, IfTrue));
                    }
                    else
                    {
                        char condition = ops[i][0];
                        char @operator = ops[i][1];
                        int value = int.Parse(ops[i].Split(@operator)[1].Split(':')[0]);
                        string IfTrue = ops[i].Split(@operator)[1].Split(':')[1];
                        wf.Operations.Add(new Ops(@operator, condition, value, IfTrue));
                    }
                }
            }

            ItemContainer initial = new ItemContainer();
            List<ItemContainer> res = new List<ItemContainer>();
            DFS("in", initial, res);

            long result = 0;

            foreach(ItemContainer c in res)
            {
                List<HashSet<int>> available = new List<HashSet<int>>() { new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), new HashSet<int>(), };
                for (int t = 0; t < 4; t++)
                    for (int i = c.Intervals[t][0]; i <= c.Intervals[t][1]; i++)
                        available[t].Add(i);
                long lresult = 1;
                for (int t = 0; t < 4; t++)
                    lresult *= (long)(available[t].Count);

                result += lresult;
            }
                                   
            return result.ToString();
        }
        public static void DFS(string wf, ItemContainer itemc, List<ItemContainer> result)
        {
            if(wf == "A" || wf == "R")
            {
                if(wf == "A")
                    result.Add(itemc);
                return;
            }
            
            Workflow current = Workflow.GetWorkflow(wf);
            for(int i = 0; i < current.Operations.Count; i++)
            {
                if(i == current.Operations.Count - 1)
                {
                    string nextw = current.Operations.Last().IfTrue!;
                    DFS(nextw, itemc, result);
                }
                else
                {
                    ItemContainer next = new ItemContainer();
                    
                    for(int t = 0; t < 4; t++)
                        for(int l = 0; l < 2; l++)
                            next.Intervals[t][l] = itemc.Intervals[t][l];

                    char trait = (char)current.Operations[i].Condition!;
                    int traitidx = trait == 'x' ? 0 : trait == 'm' ? 1 : trait == 'a' ? 2 : 3;

                    char comp = (char)current.Operations[i].Operator!;
                    int comparison = (int)current.Operations[i].Value!;

                    
                    if(comp == '<')
                    {
                        itemc.Intervals[traitidx][0] = comparison;
                        next.Intervals[traitidx][1] = comparison - 1;
                    }
                    else
                    {
                        itemc.Intervals[traitidx][1] = comparison;
                        next.Intervals[traitidx][0] = comparison + 1;
                    }
                    DFS(current.Operations[i].IfTrue!, next, result);
                }
            }
        }
        public class ItemContainer
        {
            public int[] x => Intervals[0];
            public int[] a => Intervals[1];
            public int[] m => Intervals[2];
            public int[] s => Intervals[3];

            public List<int[]> Intervals = new List<int[]>() { 
                new int[] { 1, 4000 }, 
                new int[] { 1, 4000 }, 
                new int[] { 1, 4000 }, 
                new int[] { 1, 4000 } };

            public override string ToString()
            {
                return $"[{x[0]},{x[1]}] [{a[0]},{a[1]}] [{m[0]},{m[1]}] [{s[0]},{s[1]}] ";
            }
        }
    }
}
