using System;
using System.Collections.Generic;
using Dawn;

namespace WordGrid.Core.Models
{
    /// <summary>
    /// Represents a dice with each containing side containing a character(s).
    /// </summary>
    public sealed class Dice
    {
        /// <summary>
        /// Gets and sets the visible face of the dice.
        /// </summary>
        public DiceFace VisibleFace { get; private set; }

        private IReadOnlyList<DiceFace> _faces;
        private static Random _random = new Random(); 
        private static readonly int[] _rotation = new []{ 0, 90, 180, 270 };

        /// <summary>
        /// Initialises a new instance of the <see cref="Dice" /> class.
        /// </summary>
        /// <param name="faces">Represents 6 faces of the dice.</param>
        public Dice(IReadOnlyList<DiceFace> faces)
        {
            _faces = Guard.Argument(faces, nameof(faces))
                    .NotNull()
                    .NotEmpty()
                    .Count(6)
                    .Value;

            VisibleFace = _faces[0];
        }

        /// <summary>
        /// Simulates rolling the dice and returns a face.
        /// </summary>
        public void Roll() 
        {
            var face = _faces[_random.Next(_faces.Count)];
            var rotation = _rotation[_random.Next(_rotation.Length)];
            face.ApplyRotation(rotation);
            VisibleFace = face;
        }
    }
}