using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.ComponentModel.Design;
using System.Threading.Channels;
using System.Runtime.ExceptionServices;
using Microsoft.Z3;
using System.Threading;
namespace D23
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 24;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "honestly fuck this shit?";
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
            int count = 0;

            decimal startarea = 200000000000000;
            decimal endarea =   400000000000000;

            List<Hail> hails = new List<Hail>();
            foreach(string line in lines) 
            {
                decimal[] pos = line.Split("@")[0].Trim().Split(',').ToList().ConvertAll(x => decimal.Parse(x.Trim())).ToArray();
                decimal[] v = line.Split("@")[1].Trim().Split(',').ToList().ConvertAll(x => decimal.Parse(x.Trim())).ToArray();
                hails.Add(new Hail(pos, v));
            }
            bool[] crossedalready = new bool[hails.Count];
            for(int i = 0; i < hails.Count; i++)
            {
                for(int j = i + 1; j < hails.Count; j++)
                {
                    Hail h1 = hails[i];
                    Hail h2 = hails[j];

                    bool ok = true;

                    decimal slope1 = h1.vel[1] / h1.vel[0];
                    decimal slope2 = h2.vel[1] / h2.vel[0];
                    if (slope1 == slope2)
                        continue;

                    decimal c1 = h1.pos[1] - h1.pos[0] * slope1;
                    decimal c2 = h2.pos[1] - h2.pos[0] * slope2;

                    decimal intx = (c2 - c1) / (slope1 - slope2);
                    decimal inty = slope1 * intx + c1;

                    if (intx < startarea || inty < startarea || intx > endarea || inty > endarea)
                        ok = false;

                    decimal forward1 = (inty - h1.pos[1]) / h1.vel[1];
                    decimal forward2 = (inty - h2.pos[1]) / h2.vel[1];

                    if (forward1 < 0 || forward2 < 0)
                        ok = false;

                    if (ok)
                    {
                        count++;
                    }
                        
                }
            }

            return count.ToString();
        }
        public class Hail
        {
            public decimal[] pos = new decimal[3];

            public decimal[] vel = new decimal[3];
            public Hail(decimal[] pos, decimal[] vel)
            {
                this.pos = pos;
                this.vel = vel;
            }
        }

        private static string Part2(List<string> lines)
        {

            List<Hail> hails = new List<Hail>();
            foreach (string line in lines)
            {
                decimal[] pos = line.Split("@")[0].Trim().Split(',').ToList().ConvertAll(x => decimal.Parse(x.Trim())).ToArray();
                decimal[] v = line.Split("@")[1].Trim().Split(',').ToList().ConvertAll(x => decimal.Parse(x.Trim())).ToArray();
                hails.Add(new Hail(pos, v));
            }

            string answer = "";

            using (Context ctx = new Context())
            {
                Expr rx = ctx.MkConst("rx", ctx.MkRealSort());
                Expr ry = ctx.MkConst("ry", ctx.MkRealSort());
                Expr rz = ctx.MkConst("rz", ctx.MkRealSort());

                Expr vrx = ctx.MkConst("vrx", ctx.MkRealSort());
                Expr vry = ctx.MkConst("vry", ctx.MkRealSort());
                Expr vrz = ctx.MkConst("vrz", ctx.MkRealSort());

                Expr t1 = ctx.MkConst("t1", ctx.MkRealSort());
                Expr t2 = ctx.MkConst("t2", ctx.MkRealSort());
                Expr t3 = ctx.MkConst("t3", ctx.MkRealSort());

                Expr h1x = ctx.MkNumeral(hails[0].pos[0].ToString(), ctx.MkRealSort());
                Expr h1y = ctx.MkNumeral(hails[0].pos[1].ToString(), ctx.MkRealSort());
                Expr h1z = ctx.MkNumeral(hails[0].pos[2].ToString(), ctx.MkRealSort());

                Expr h2x = ctx.MkNumeral(hails[1].pos[0].ToString(), ctx.MkRealSort());
                Expr h2y = ctx.MkNumeral(hails[1].pos[1].ToString(), ctx.MkRealSort());
                Expr h2z = ctx.MkNumeral(hails[1].pos[2].ToString(), ctx.MkRealSort());

                Expr h3x = ctx.MkNumeral(hails[2].pos[0].ToString(), ctx.MkRealSort());
                Expr h3y = ctx.MkNumeral(hails[2].pos[1].ToString(), ctx.MkRealSort());
                Expr h3z = ctx.MkNumeral(hails[2].pos[2].ToString(), ctx.MkRealSort());

                Expr vh1x = ctx.MkNumeral(hails[0].vel[0].ToString(), ctx.MkRealSort());
                Expr vh1y = ctx.MkNumeral(hails[0].vel[1].ToString(), ctx.MkRealSort());
                Expr vh1z = ctx.MkNumeral(hails[0].vel[2].ToString(), ctx.MkRealSort());

                Expr vh2x = ctx.MkNumeral(hails[1].vel[0].ToString(), ctx.MkRealSort());
                Expr vh2y = ctx.MkNumeral(hails[1].vel[1].ToString(), ctx.MkRealSort());
                Expr vh2z = ctx.MkNumeral(hails[1].vel[2].ToString(), ctx.MkRealSort());

                Expr vh3x = ctx.MkNumeral(hails[2].vel[0].ToString(), ctx.MkRealSort());
                Expr vh3y = ctx.MkNumeral(hails[2].vel[1].ToString(), ctx.MkRealSort());
                Expr vh3z = ctx.MkNumeral(hails[2].vel[2].ToString(), ctx.MkRealSort());


                Solver s = ctx.MkSolver();
                s.Assert(
                    ctx.MkAnd(
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rx, ctx.MkMul((ArithExpr)t1, (ArithExpr)vrx)),
                            ctx.MkAdd((ArithExpr)h1x, ctx.MkMul((ArithExpr)t1, (ArithExpr)vh1x))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)ry, ctx.MkMul((ArithExpr)t1, (ArithExpr)vry)),
                            ctx.MkAdd((ArithExpr)h1y, ctx.MkMul((ArithExpr)t1, (ArithExpr)vh1y))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rz, ctx.MkMul((ArithExpr)t1, (ArithExpr)vrz)),
                            ctx.MkAdd((ArithExpr)h1z, ctx.MkMul((ArithExpr)t1, (ArithExpr)vh1z))
                        ),


                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rx, ctx.MkMul((ArithExpr)t2, (ArithExpr)vrx)),
                            ctx.MkAdd((ArithExpr)h2x, ctx.MkMul((ArithExpr)t2, (ArithExpr)vh2x))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)ry, ctx.MkMul((ArithExpr)t2, (ArithExpr)vry)),
                            ctx.MkAdd((ArithExpr)h2y, ctx.MkMul((ArithExpr)t2, (ArithExpr)vh2y))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rz, ctx.MkMul((ArithExpr)t2, (ArithExpr)vrz)),
                            ctx.MkAdd((ArithExpr)h2z, ctx.MkMul((ArithExpr)t2, (ArithExpr)vh2z))
                        ),


                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rx, ctx.MkMul((ArithExpr)t3, (ArithExpr)vrx)),
                            ctx.MkAdd((ArithExpr)h3x, ctx.MkMul((ArithExpr)t3, (ArithExpr)vh3x))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)ry, ctx.MkMul((ArithExpr)t3, (ArithExpr)vry)),
                            ctx.MkAdd((ArithExpr)h3y, ctx.MkMul((ArithExpr)t3, (ArithExpr)vh3y))
                        ),
                        ctx.MkEq(
                            ctx.MkAdd((ArithExpr)rz, ctx.MkMul((ArithExpr)t3, (ArithExpr)vrz)),
                            ctx.MkAdd((ArithExpr)h3z, ctx.MkMul((ArithExpr)t3, (ArithExpr)vh3z))
                        )

                    )
                );

                s.Check();

                Model m = s.Model;
                answer = (decimal.Parse(m.ConstInterp(rx).ToString()) + decimal.Parse(m.ConstInterp(ry).ToString()) + decimal.Parse(m.ConstInterp(rz).ToString())).ToString();    
                
            }
            return answer;
        }
    }
}
