using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Chunky
{
    public class FlattenedEnumerable<T> :
        IEnumerable<T>, IEnumerable, IChunkEnumerable<T>
    {
        private readonly IEnumerable<Chunk<T>> _chunks;

        public FlattenedEnumerable(IEnumerable<Chunk<T>> chunks)
        {
            _chunks = chunks;
        }

        public IEnumerator<T> GetEnumerator() =>
            _chunks.Flatten().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();

        IEnumerator<Chunk<T>> IChunkEnumerable<T>.GetChunkEnumerator() =>
            _chunks.GetEnumerator();
    }
}