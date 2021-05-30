using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using PokerHandEvaluator;

namespace SixCardOmahaOddsCalculator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<bool>(
                    "-c",
                    getDefaultValue: () => false,
                    description: "color"),
                new Argument<FileInfo>(
                    "input-file",
                    getDefaultValue: () => null,
                    description: "input file").ExistingOnly()
            };

            rootCommand.Description = "6 Card Omaha Odds Calculator";

            rootCommand.Handler = CommandHandler.Create<bool, FileInfo>((c, inputFile) =>
                {
                    if (inputFile == null)
                    {
                        Calc(() => GetInputFromConsole(), c);
                    }
                    else
                    {
                        Calc(() => GetInputFromFile(inputFile.FullName), c);
                    }

                    return 0;
                });

            return rootCommand.Invoke(args);
        }

        public static void Calc(Func<CalcInput> inputFunc, bool isColor)
        {
            var ci = inputFunc();
            var r = CalcOddsOmaha6.Calc(ci);
            OutputResult(r, isColor);
        }

        public static CalcInput GetInputFromConsole()
        {
            Console.Write("Community Card:");
            var communityText = Console.ReadLine();

            var communityCardNames = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                if (i * 2 + 2 <= communityText.Length)
                {
                    communityCardNames.Add(communityText.Substring(i * 2, 2));
                }
                else
                {
                    break;
                }
            }


            Console.Write("Except Card:");
            var exceptText = Console.ReadLine();

            var exceptCardNames = new List<string>();
            for (var i = 0; i < exceptText.Length; i += 2)
            {
                if (i + 2 <= exceptText.Length)
                {
                    exceptCardNames.Add(exceptText.Substring(i, 2));
                }
                else
                {
                    break;
                }
            }


            var playersCardNames = new List<List<string>>();
            for (var i = 0; i < 6; i++) //6-Max
            {
                Console.Write($"Player {i + 1} Card:");
                var playerText = Console.ReadLine();

                if (playerText == "")
                {
                    break;
                }

                var playerCardNames = new List<string>();
                for (var j = 0; j < 6; j++)
                {
                    if (j * 2 + 2 <= playerText.Length)
                    {
                        playerCardNames.Add(playerText.Substring(j * 2, 2));
                    }
                    else
                    {
                        throw new ArgumentException("must : 6 card : " + playerText);
                    }
                }
                playersCardNames.Add(playerCardNames);
            }
            if (playersCardNames.Count < 2)
            {
                throw new ArgumentException("must : 2 <= player count : " + (playersCardNames.Count - 1).ToString());
            }

            return new CalcInput(communityCardNames, exceptCardNames, playersCardNames);
        }

        public static CalcInput GetInputFromFile(string path)
        {
            var textList = new List<string>();
            var exceptText = "";

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    var line = sr.ReadLine();
                    if (line.StartsWith("-"))
                    {
                        exceptText = line;
                    }
                    else
                    {
                        textList.Add(line);
                    }
                }
            }

            if (textList.Count - 1 < 2)
            {
                throw new ArgumentException("must : 2 <= player count : " + (textList.Count - 1).ToString());
            }

            var communityCardNames = new List<string>();

            var communityText = textList[0];
            for (var i = 0; i < 5; i++)
            {
                if (i * 2 + 2 <= communityText.Length)
                {
                    communityCardNames.Add(communityText.Substring(i * 2, 2));
                }
                else
                {
                    break;
                }
            }

            var playersCardNames = new List<List<string>>();
            for (var i = 0; i < 6; i++) //6-Max
            {
                if (i <= textList.Count - 2)
                {
                    var playerCardNames = new List<string>();
                    var playerText = textList[i + 1];
                    for (var j = 0; j < 6; j++)
                    {
                        if (j * 2 + 2 <= playerText.Length)
                        {
                            playerCardNames.Add(playerText.Substring(j * 2, 2));
                        }
                        else
                        {
                            throw new ArgumentException("must : 6 card : " + playerText);
                        }
                    }
                    playersCardNames.Add(playerCardNames);
                }
            }

            var exceptCardNames = new List<string>();
            for (var i = 1; i < exceptText.Length; i += 2)
            {
                if (i + 2 <= exceptText.Length)
                {
                    exceptCardNames.Add(exceptText.Substring(i, 2));
                }
                else
                {
                    break;
                }
            }

            return new CalcInput(communityCardNames, exceptCardNames, playersCardNames);
        }

        public static void OutputResult(CalcResult r, bool isColor)
        {
            OutputCard("board", r.CommunityCard, "", isColor);

            if (0 < r.ExceptCard.Count)
            {
                OutputCard("except", r.ExceptCard, "", isColor);
            }

            var sumWinPoint = r.Players.Select(e => e.GetWinPoint()).Sum();
            foreach (var p in r.Players)
            {
                var desc = "";
                if (r.CommunityCard.Count == 5)
                {
                    desc = " " + p.GetRiverRank().DescribeRank();
                }
                var winOdds = (decimal)p.GetWinPoint() / sumWinPoint * 100;
                OutputCard("", p.GetHoleCard(), $" ({winOdds,6:0.00}%){desc}", isColor);
            }
        }

        public static void OutputCard(string before, List<Card> c, string after, bool isColor)
        {
            if (isColor)
            {
                OutputCardColor(before, c, after);
            }
            else
            {
                OutputCardDefault(before, c, after);
            }
        }

        public static void OutputCardDefault(string before, List<Card> c, string after)
        {
            Console.WriteLine(before + "[" + String.Join(" ", c.Select(e => e.ToString()).ToArray()) + "]" + after);
        }

        public static void OutputCardColor(string before, List<Card> c, string after)
        {
            Console.Write(before + "[");

            var s = "";
            foreach (var i in c)
            {
                Console.Write(s);

                var n = i.ToString();
                switch (n[1])
                {
                    case 'c':
                    case 'C':
                        Console.ForegroundColor = ConsoleColor.Green;

                        break;
                    case 'd':
                    case 'D':
                        Console.ForegroundColor = ConsoleColor.Blue;

                        break;
                    case 'h':
                    case 'H':
                        Console.ForegroundColor = ConsoleColor.Red;

                        break;
                    case 's':
                    case 'S':

                        break;
                    default:
                        throw new ArgumentException(i.ToString());
                }

                Console.Write(i.ToString());

                Console.ResetColor();
                s = " ";
            }

            Console.WriteLine("]" + after);
        }
    }
}
