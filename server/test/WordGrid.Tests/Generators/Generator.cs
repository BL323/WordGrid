using System;

namespace WordGrid.Tests.Generators 
{
    /// <summary>
    /// Credit: https://blog.ploeh.dk/2017/09/18/the-test-data-generator-functor/
    /// </summary>
    public class Generator<T>
    {
        private readonly Func<Random, T> generate;
    
        public Generator(Func<Random, T> generate)
        {
            if (generate == null)
                throw new ArgumentNullException(nameof(generate));
    
            this.generate = generate;
        }
    
        public Generator<T1> Select<T1>(Func<T, T1> f)
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));
    
            Func<Random, T1> newGenerator = r => f(this.generate(r));
            return new Generator<T1>(newGenerator);
        }
    
        public T Generate(Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
    
            return this.generate(random);
        }
    }
}