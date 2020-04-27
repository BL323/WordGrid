using WordGrid.Core.Models;

namespace WordGrid.Tests.Generators 
{
    public static class Gens
    {
        private const string InputCharacters 
            = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,R,S,T,U,V,W,X,Y,Z,QU";

        public static Generator<DiceFace> DiceFace 
            = new Generator<DiceFace>(r => 
            {
                var split = InputCharacters.Split(',');
                return new DiceFace(split[r.Next(split.Length)]);
            });
    }
}