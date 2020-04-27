using System;
using System.Linq;
using Dawn;

namespace WordGrid.Core.Utils 
{
    public static class GuardExtensions
    {
        public static Guard.ArgumentInfo<string> IsValidCharacters(
            in this Guard.ArgumentInfo<string> argument)
        {
            if(!argument.Value.All(x => Char.IsLetter(x)))
                throw Guard.Fail(new ArgumentException(
                    $"{argument.Name} contains characters other than letters. " +
                    "Please use a single letter other than the special case 'QU'.",
                    argument.Name));

            if(argument.Value.Equals("QU", StringComparison.InvariantCultureIgnoreCase))
                return argument;

            if(argument.Value.Length != 1)
                   throw Guard.Fail(new ArgumentException(
                    $"{argument.Name} must only be 1 character long." +
                    "Please use a single letter other than the special case 'QU'.",
                    argument.Name));

            if(argument.Value.Equals("Q"))
               throw Guard.Fail(new ArgumentException(
                    $"{argument.Name} cannot be the single letter 'Q'." +
                    "'Q' should be used as 'Qu' to ensure more valid words can be made.",
                    argument.Name));

            return argument;
        }
    }
}