using System;
using System.Collections.Generic;
using System.Linq;
using Dawn;

namespace WordGrid.Core.Models
{
    /// <summary>
    /// Represents the 4 x 4 grid containing dice.
    /// </summary>
    public sealed class Grid
    {
        /// <summary>
        /// Gets the grid size.
        /// </summary>
        public int GridSize { get; }

        /// <summary>
        /// Gets the current representation of dice on the grid.
        /// </summary>
        public IReadOnlyList<Dice> Dice => _dice;

        private List<Dice> _dice;

        /// <summary>
        /// Initialises a new instance of the <see cref="Grid" /> class.
        /// </summary>
        /// <param name="dice">The dice to be placed on the grid.</param>
        internal Grid(IReadOnlyList<Dice> dice)
        {
            _dice = Guard.Argument(dice, nameof(dice))
                .NotNull()
                .Count(16)
                .Value
                .ToList();

            GridSize = 4;
        }

        /// <summary>
        /// Shuffles the positions of each dice and rolls to alter the grid.
        /// </summary>
        public void ShuffleAndRoll()
        {
            var shuffled = Shuffle(_dice);
            foreach(var dice in shuffled)
                dice.Roll();

            _dice = new List<Dice>(shuffled);
        }

        private List<Dice> Shuffle(List<Dice> dice)
            => new List<Dice>(dice).OrderBy(d => Guid.NewGuid()).ToList();
    }
}