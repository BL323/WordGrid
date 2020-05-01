using System;
using System.Collections.Generic;

namespace WordGrid.Api.Client.Interface
{
    public class Game
    {
        public Guid ID { get; set; }
        public GameStateEnum GameState { get; set; }
        public int GridSize { get; set; }
        public List<Dice> Grid { get; set; }
        public int CurrentRound { get; set; }
        public int RoundsToPlay { get; set; }
        public int CountdownLength { get; set; }
        public double RoundTimeRemaining { get; set; }
    }
}