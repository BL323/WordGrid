using System;
using System.Threading.Tasks;
using Moq;
using WordGrid.Core.Domain;
using WordGrid.Core.Models;
using Xunit;

namespace WordGrid.Tests
{
    public class GameTests 
    {
        [Fact]
        public void CreateGame_SucceedsWith_ValidInput() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            new Game(eventPublisher.Object, grid);
        }

        [Fact]
        public void CreateGame_FailsWith_NegativeRoundsToPlay() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            Assert.Throws<ArgumentOutOfRangeException>(() => new Game(eventPublisher.Object, grid, -9));
        }

        [Fact]
        public void CreateGame_FailsWith_LowerThanThresholdSecondsPerRound() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            Assert.Throws<ArgumentOutOfRangeException>(() => new Game(eventPublisher.Object, grid, 5, 2));
        }

        [Fact]
        public void ValidState_SucceedsWith_NoRoundsPlayed() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            var game = new Game(eventPublisher.Object, grid);
            Assert.Equal(0, game.NumberOfPlayedRounds);
        }

        [Fact]
        public async Task ValidState_SucceedsWith_ThreeRoundsPlayed() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            var game = new Game(eventPublisher.Object, grid);
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            Assert.Equal(3, game.NumberOfPlayedRounds);
        }

        [Fact]
        public async Task MaxThreeRoundsPlayed_SucceedsWith_ThreeRoundGameLength() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            var game = new Game(eventPublisher.Object, grid, 3);
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            Assert.Equal(3, game.NumberOfPlayedRounds);
        }

        [Fact]
        public async Task FinishedState_SucceedsWith_AllRoundsPlayed() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            var game = new Game(eventPublisher.Object, grid, 3);
            await game.NextRoundAsync();
            await game.NextRoundAsync();
            await game.NextRoundAsync();

            await game.FinishGameAsync();

            Assert.Equal(WordGrid.Core.Models.State.Finished, game.State);
        }

        [Fact]
        public async Task FinishedState_SucceedsWith_NotAllRoundsPlayed() 
        {
            Mock<IEventPublisher> eventPublisher = new Mock<IEventPublisher>();
            Grid grid = new GridFactory().BuildWithClassicDice();

            var game = new Game(eventPublisher.Object, grid, 3);
            await game.NextRoundAsync();
            await game.FinishGameAsync();

            Assert.Equal(WordGrid.Core.Models.State.Finished, game.State);
        }
    }
}