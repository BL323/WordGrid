using Xunit;
using System;
using WordGrid.Tests.Generators;
using System.Linq;
using WordGrid.Core.Models;

namespace WordGrid.Tests
{
    public class DiceTests
    {
        // Use seed of random to ensure tests are deterministic.
        private static Random _random = new Random(3);

        [Fact]
        public void CreateDice_SucceedsWith_6Faces()
        {
            var diceFaces = Enumerable
                .Range(0, 6)
                .Select(x => Gens.DiceFace.Generate(_random))
                .ToArray();
            
            new Dice(diceFaces);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(9)]
        public void CreateDice_FailsWith_Not6Faces(int numFaces)
        {
            var diceFaces = Enumerable
                .Range(0, numFaces)
                .Select(x => Gens.DiceFace.Generate(_random))
                .ToArray();
            
            Assert.Throws<ArgumentException>(() => new Dice(diceFaces));
        }

        [Fact]
        public void CreateDice_FailsWith_NullInput()
        {
            Assert.Throws<ArgumentNullException>(() => new Dice(null));
        }
    } 
}