using System.Runtime.Intrinsics;
using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D07
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 7;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "coooookiiiieeee :3";
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

            List<Hand> hands = new List<Hand>();
            foreach(string line in lines)
            {
                string hand = line.Split(' ')[0];
                int bid = int.Parse(line.Split(' ')[1]);
                hands.Add(new Hand(hand, bid, 1));
            }

            hands.Sort((x1, x2) => x1.CompareHand(x2,1));

            for(int i = 0; i < hands.Count; i++)
            {
                toreturn += hands[i].Bid * (i + 1);
            }

            return toreturn.ToString();
        }
        public class Hand
        { 
            public int[] v = new int[15];
            public int Bid = 0;
            public int Rank;
            public string c = "";
            public string initial;
            public Hand(string cards, int bid, int part)
            {
                Bid = bid;
                initial = cards;
                for(int i = 0; i < cards.Length; i++)
                {
                    v[GetCard(cards[i].ToString(), part)]++;
                }

                if (v[1] > 0)
                {
                    int bestidx = 1;
                    int max = -1;
                    for(int i = 14; i >= 2; i--)
                    {
                        if (v[i] > max)
                        {
                            bestidx = i;
                            max = v[i];
                        }
                    }
                    if(bestidx != 1)
                    {
                        v[bestidx] += v[1];
                        v[1] = 0;
                    }
                }
                GetScore();
            }
            public override string ToString()
            {
                return initial;
            }
            public bool FiveOfAKind(out int idx)
            {
                idx = -1;
                for(int i = 0; i < v.Length; i++)
                {
                    if (v[i] == 5)
                    {
                        idx = i;
                        return true;
                    }
                }
                return false;
            }
            public bool FourOfAKind(out int idx)
            {
                idx = -1;
                for (int i = 0; i < v.Length; i++)
                {
                    if (v[i] == 4)
                    {
                        idx = i;
                        return true;
                    }
                }
                return false;
            }
            public bool FullHouse(out List<int> idxs)
            {
                int idx3 = -1;
                int idx2 = -1;
                idxs = new List<int>() { idx3, idx2 };
                return ThreeOfAKind(out idx3) && TwoOfAKind(out idx2, idx3);
                
            }
            public bool ThreeOfAKind(out int idx, int withoutidx = -1)
            {
                idx = -1;
                for (int i = 0; i < v.Length; i++)
                {
                    if (i == withoutidx)
                        continue;
                    if (v[i] == 3)
                    {
                        idx = i;
                        return true;
                    }
                }
                return false;
            }
            public bool TwoOfAKind(out int idx, int withoutidx = -1)
            {
                idx = -1;
                for (int i = 0; i < v.Length; i++)
                {
                    if (i == withoutidx)
                        continue;
                    if (v[i] == 2)
                    {
                        idx = i;
                        return true;
                    }
                }
                return false;
            }
            public bool TwoPairs()
            {
                return TwoOfAKind(out int idx) && TwoOfAKind(out int _, idx);
            }
            public int GetCard(string s, int part)
            {
                if (s == "A")
                    return 14;
                else if (s == "K")
                    return 13;
                else if (s == "Q")
                    return 12;
                else if (s == "J")
                {
                    if (part == 1)
                        return 11;
                    else
                        return 1;
                }
                else if (s == "T")
                    return 10;
                else
                    return int.Parse(s);
            }
            public void GetScore()
            {
                if (FiveOfAKind(out int _))
                    Rank = 10;
                else if (FourOfAKind(out int _))
                    Rank = 9;
                else if (FullHouse(out _))
                    Rank = 8;
                else if (ThreeOfAKind(out int _))
                    Rank = 7;
                else if (TwoPairs())
                    Rank = 6;
                else if (TwoOfAKind(out int _))
                    Rank = 5;
                else
                    Rank = 4;
            }
            public int CompareHand(Hand other, int part)
            {
                if (this.Rank != other.Rank)
                    return this.Rank.CompareTo(other.Rank);
                else
                {
                    for (int i = 0; i < initial.Length; i++) 
                    {
                        int thisscore = GetCard(initial[i].ToString(), part);
                        int otherscore = GetCard(other.initial[i].ToString(), part);
                        if( thisscore != otherscore )
                            return thisscore.CompareTo( otherscore );
                    }
                }
                return 0;
            }
            //public int CompareHand2(Hand other)
            //{
            //    if (this.Rank != other.Rank)
            //        return this.Rank.CompareTo(other.Rank);
            //    else
            //    {
            //        int maxsc = 0;
            //        switch(Rank)
            //        {
            //            case 10:
            //                maxsc = 5;
            //                break;
            //            case 9:
            //                maxsc = 4;
            //                break;
            //            case 8:
            //            case 7:
            //                maxsc = 3;
            //                break;
            //            case 6:
            //            case 5:
            //                maxsc = 2;
            //                break;
            //            default:
            //                maxsc = 1;
            //                break;
            //        }
            //        while(maxsc > 0)
            //        {
            //            int idx = 14;
            //            while (idx >= 0)
            //            {
            //                if (((v[idx] == maxsc && v[idx] > 0) || (other.v[idx] == maxsc && other.v[idx] > 0)) && v[idx] != other.v[idx])
            //                    break;
            //                idx--;
            //            }
            //            if (idx == -1)
            //                maxsc--;
            //            else
            //                return v[idx].CompareTo(other.v[idx]);
            //        }                                      
            //    }
            //    return 0;
            //}
        }
        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            List<Hand> hands = new List<Hand>();
            foreach (string line in lines)
            {
                string hand = line.Split(' ')[0];
                int bid = int.Parse(line.Split(' ')[1]);
                hands.Add(new Hand(hand, bid, 2));
            }

            hands.Sort((x1, x2) => x1.CompareHand(x2, 2));

            for (int i = 0; i < hands.Count; i++)
            {
                toreturn += hands[i].Bid * (i + 1);
            }

            return toreturn.ToString();
        }
    }
}
