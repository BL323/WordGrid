using System.Collections.Generic;
using WordGrid.Core.Models;
using Xunit;

namespace WordGrid.Tests
{
    public class GridFactoryTests
    {
        [Fact]
        public void BuildGrid_SucceedsWith_16PopulatedDice()
        {
            var factory = new GridFactory();
            Grid grid = factory.BuildWithClassicDice();
            
            Assert.NotNull(grid);
            Assert.Equal(16, grid.Dice.Count);
        }
    }
}