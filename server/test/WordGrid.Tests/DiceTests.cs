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
            
            new Dice(0, diceFaces);
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
            
            Assert.Throws<ArgumentException>(() => new Dice(0, diceFaces));
        }

        [Fact]
        public void CreateDice_FailsWith_NullInput()
        {
            Assert.Throws<ArgumentNullException>(() => new Dice(0, null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(15)]
        public void CreateDice_SucceedsWith_ValidPosition(int position)
        {
            var validFaces = Enumerable
                .Range(0, 6)
                .Select(x => Gens.DiceFace.Generate(_random))
                .ToArray();

            new Dice(position, validFaces);
        }

        [Fact]
        public void CreateDice_FailsWith_NegativePosition()
        {
            var validFaces = Enumerable
                .Range(0, 6)
                .Select(x => Gens.DiceFace.Generate(_random))
                .ToArray();

            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(-1, validFaces));
        }

        [Fact]
        public void CreateDice_FailsWith_PositionGreaterThan15()
        {
            var validFaces = Enumerable
                .Range(0, 6)
                .Select(x => Gens.DiceFace.Generate(_random))
                .ToArray();

            Assert.Throws<ArgumentOutOfRangeException>(() => new Dice(16, validFaces));
        }
    } 
}