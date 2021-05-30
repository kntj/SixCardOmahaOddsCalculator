using System.Collections.Generic;
using PokerHandEvaluator;

namespace SixCardOmahaOddsCalculator
{
    public class Player
    {
        int winPoint = 0;
        List<Card> holeCard;
        Rank riverRank = null;

        public Player() { }

        public Player(List<Card> c)
        {
            SetHoleCard(c);
        }

        public void SetHoleCard(List<Card> h)
        {
            holeCard = h;
        }

        public List<Card> GetHoleCard()
        {
            return holeCard;
        }

        public void AddWinPoint(int wp)
        {
            winPoint = winPoint + wp;
        }

        public int GetWinPoint()
        {
            return winPoint;
        }

        public void SetRiverRank(Rank r)
        {
            riverRank = r;
        }

        public Rank GetRiverRank()
        {
            return riverRank;
        }
    }
}