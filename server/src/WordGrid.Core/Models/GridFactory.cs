using System;
using System.Collections.Generic;
using System.Linq;

namespace WordGrid.Core.Models
{
    /// <summary>
    /// Builds grid with various configurations of dice.
    /// </summary>
    public class GridFactory 
    {
        /// <summary>
        /// Returns a grid populated with the classic letter combination for each dice.
        /// </summary>
        public Grid BuildWithClassicDice()
        {
            var classicDiceLettering = new string[16,6] 
            {
                { "P", "S", "H", "A", "O", "C" },
                { "N", "G", "E", "E", "A", "A" },
                { "A", "S", "P", "F", "K", "F" },
                { "W", "E", "G", "N", "H", "E" },
                { "A", "B", "B", "O", "J", "O" },
                { "E", "O", "T", "S", "I", "S" },
                { "R", "H", "Z", "L", "N", "N" },
                { "R", "L", "I", "E", "D", "X" },
                { "O", "O", "T", "W", "A", "T" },
                { "E", "Y", "L", "D", "R", "V" },
                { "D", "T", "Y", "T", "I", "S" },
                { "I", "U", "N", "H", "M", "QU" },
                { "T", "Y", "E", "L", "R", "T" },
                { "C", "M", "U", "O", "T", "I" },
                { "S", "U", "E", "E", "N", "I" },
                { "H", "E", "V", "W", "T", "R" },
            };

            var dice = new List<Dice>();
            for(var i = 0; i < 16; i++) 
            {
                var faces = new List<DiceFace>();
                for(var j = 0; j < 6; j++) 
                {
                    faces.Add(new DiceFace(classicDiceLettering[i, j]));
                }
                dice.Add(new Dice(position: i, faces));
            }

            return new Grid(dice);
        }

        /// <summary>
        /// Returns a grid populated with a completely random collection of letters.
        /// </summary>
        public Grid BuildWithRandomDice()
        {
            throw new NotImplementedException();
        }
    }
}