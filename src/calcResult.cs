using System.Collections.Generic;
using PokerHandEvaluator;

namespace SixCardOmahaOddsCalculator
{
    public class CalcResult
    {
        public List<Card> CommunityCard;
        public List<Card> ExceptCard;
        public List<Player> Players;

        public CalcResult()
        {
            CommunityCard = new List<Card>();
            ExceptCard = new List<Card>();
            Players = new List<Player>();
        }

        public CalcResult(List<Card> communityCard, List<Card> exceptCard, List<Player> players)
        {
            CommunityCard = communityCard;
            ExceptCard = exceptCard;
            Players =  players; 
        }
    }
}