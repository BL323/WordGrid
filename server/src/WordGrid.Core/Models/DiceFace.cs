using System;
using System.Linq;
using Dawn;
using WordGrid.Core.Utils;

namespace WordGrid.Core.Models
{
    /// <summary>
    /// Represents a face of dice.
    /// </summary>
    public sealed class DiceFace 
    {
        /// <summary>
        /// Gets lexical value of the character(s).
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets wether the character requires underlining for clarity when rotated. 
        /// </summary>
        public bool ShouldUnderline { get; }

        /// <summary>
        /// Gets the rotation of the dice face once it's been rolled.
        /// </summary>
        public int Rotation { get; private set; }

        private static readonly string[] UnderlinedCharacters = new string[] 
        { 
            "M", "W", "N", "Z",
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="DiceFace" /> class.
        /// </summary>
        /// <param name="value"></param>
        public DiceFace(string value)
        {
            Value = Guard.Argument(value, nameof(value))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace()
                .MaxLength(2)
                .IsValidCharacters()
                .Value
                .ToUpper();

            ShouldUnderline = RequiresUnderline(Value);
        }

        internal void ApplyRotation(int rotation)
            => Rotation = rotation;

        private bool RequiresUnderline(string value)
            => UnderlinedCharacters.Contains(value);
    }
}