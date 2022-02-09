using System.Collections.Generic;

namespace Chunky
{
    /// <summary>
    /// A sequence that is composed of underlying chunks.
    /// </summary>
    public interface IChunkEnumerable<T>
    {
        public IEnumerator<Chunk<T>> GetChunkEnumerator();
    }
}