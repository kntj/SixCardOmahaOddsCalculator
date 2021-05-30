using System;
using System.Collections.Generic;
using System.Linq;
using PokerHandEvaluator;

namespace SixCardOmahaOddsCalculator
{
    public class CalcOddsOmaha6
    {
        public static CalcResult Calc(CalcInput ci)
        {
            var players = ci.PlayersCard.Select(e => new Player(e)).ToList();
            var communityCard = ci.CommunityCard;
            var exceptCard = ci.ExceptCard;

            var usedCard = players.SelectMany(e => e.GetHoleCard()).Concat(communityCard).Concat(exceptCard).ToList();
            if (usedCard.Count() != usedCard.Select(e => (int)e).Distinct().Count())
            {
                var sameCard = usedCard.Where(e => 2 <= usedCard.Where(x => (int)e == (int)x).Count()).ToList();
                throw new ArgumentException("same name : " + String.Join(" ", sameCard.Select(e => e.ToString()).ToArray()));
            }

            var dealerCard = Deck.All().Where(e => !usedCard.Any(x => (int)e == (int)x)).ToList();

            Console.WriteLine($"combinations:{Util.Choose(dealerCard.Count, 5 - communityCard.Count)}");

            IEnumerable<List<Card>> allCaseList;
            if (communityCard.Count == 5)
            {
                allCaseList = Enumerable.Range(0, 1).Select(e => new List<Card>()); //length 1 dummy
            }
            else
            {
                allCaseList = Util.Combinations(dealerCard, 5 - communityCard.Count);
            }
            var allCase5CardList = allCaseList.Select(e => {
                                                            e.AddRange(communityCard); 
                                                            return e;});

            var ranks = new Rank[players.Count];
            foreach (var c in allCase5CardList)
            {
                for (var i = 0; i < ranks.Count(); i++)
                {
                    var h = players[i].GetHoleCard();
                    ranks[i] = Evaluator.EvaluateOmaha6Cards(c[0], c[1], c[2], c[3], c[4], h[0], h[1], h[2], h[3], h[4], h[5]);
                }

                var bestRank = ranks.Max();

                var bestRankCount = ranks.Where(r => r == bestRank).Count();

                var winPoint = 60 / bestRankCount; //6-Max

                for (var i = 0; i < ranks.Count(); i++)
                {
                    if (ranks[i] == bestRank)
                    {
                        players[i].AddWinPoint(winPoint);
                    }
                }
            }

            if (communityCard.Count == 5)
            {
                for (var i = 0; i < ranks.Count(); i++)
                {
                    players[i].SetRiverRank(ranks[i]);
                }
            }

            return new CalcResult(communityCard, exceptCard, players);
        }
    }
}
