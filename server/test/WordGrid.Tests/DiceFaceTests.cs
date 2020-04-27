using System;
using WordGrid.Core.Models;
using Xunit;

namespace WordGrid.Tests
{
    public class DiceFaceTests
    {
        [Theory]
        [InlineData("A")]
        [InlineData("F")]
        [InlineData("X")]
        public void CreateDiceFace_SucceedsWith_UpperCaseSingleCharacter(string character)
        {
            new DiceFace(character);
        }

        [Theory]
        [InlineData("j")]
        [InlineData("k")]
        [InlineData("z")]
        public void CreateDiceFace_SucceedsWith_LowerCaseSingleCharacter(string character)
        {
            new DiceFace(character);
        }

        [Theory]
        [InlineData("QU")]
        [InlineData("Qu")]
        [InlineData("qu")]
        public void CreateDiceFace_SucceedsWith_SpecialQuCharacters(string quCharacters)
        {
            new DiceFace(quCharacters);
        }
        
        [Fact]
        public void CreateDiceFace_FailsWith_SingleQCharacter()
        {
            Assert.Throws<ArgumentException>(() => new DiceFace("Q"));
        }

        [Fact]
        public void CreateDiceFace_FailsWith_NullInput()
        {
            Assert.Throws<ArgumentNullException>(() => new DiceFace(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateDiceFace_FailsWith_EmptyOrWhitespaceInput(string characters)
        {
            Assert.Throws<ArgumentException>(() => new DiceFace(characters));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("!")]
        [InlineData("}")]
        [InlineData(".")]
        public void CreateDiceFace_FailsWith_CharactersThatAreNotLetters(string character)
        {
            Assert.Throws<ArgumentException>(() => new DiceFace(character));
        }
    }
}
