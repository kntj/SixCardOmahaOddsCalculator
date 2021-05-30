using System.Collections.Generic;
using System.Linq;
using PokerHandEvaluator;

namespace SixCardOmahaOddsCalculator
{
    public class CalcInput
    {
        public List<Card> CommunityCard;
        public List<Card> ExceptCard;
        public List<List<Card>> PlayersCard;

        public CalcInput()
        {
            CommunityCard = new List<Card>();
            ExceptCard = new List<Card>();
            PlayersCard = new List<List<Card>>();
        }

        public CalcInput(List<Card> communityCard, List<Card> exceptCard, List<List<Card>> playersCard)
        {
            CommunityCard = communityCard;
            ExceptCard = exceptCard;
            PlayersCard =  playersCard;
        }

        public CalcInput(List<string> communityCardNames, List<string> exceptCardNames, List<List<string>> playersCardNames)
        {
            CommunityCard = communityCardNames.Select(e => new Card(e)).ToList();
            ExceptCard = exceptCardNames.Select(e => new Card(e)).ToList();
            PlayersCard =  playersCardNames.Select(e => e.Select(x => new Card(x)).ToList()).ToList();
        }
    }
}