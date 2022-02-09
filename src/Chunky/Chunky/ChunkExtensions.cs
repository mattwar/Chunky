using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Chunky
{
    public static class ChunkExtensions
    {
        /// <summary>
        /// Flattens a sequence of chunks in linear time.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<Chunk<T>> items)
        {
            return FlattenChunks<T>(items.GetEnumerator());
        }

        /// <summary>
        /// Flattens a sequence of chunks in linear time.
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerator<Chunk<T>> items)
        {
            return FlattenChunks<T>(items);
        }

        private static IEnumerable<T> FlattenChunks<T>(IEnumerator itemEnum)
        {
            var stack = new Stack<IEnumerator>();
            stack.Push(itemEnum);

            while (stack.Count > 0)
            {
                var top = stack.Peek();

                if (top is IEnumerator<Chunk<T>> chunkyEnum)
                {
                    if (chunkyEnum.MoveNext())
                    {
                        var chunk = chunkyEnum.Current;

                        if (chunk.TryGetValue(out var tValue))
                        {
                            yield return tValue;
                        }
                        else if (chunk.TryGetChunkSequence(out var chunkySeq))
                        {
                            stack.Push(chunkySeq.GetEnumerator());
                        }
                        else if (chunk.TryGetChunkEnumerator(out var chunkEnum))
                        {
                            stack.Push(chunkEnum);
                        }
                        else if (chunk.TryGetSequence(out var seq))
                        {
                            stack.Push(seq.GetEnumerator());
                        }
                    }
                    else
                    {
                        stack.Pop();
                        chunkyEnum.Dispose();
                    }
                }
                else if (top is IEnumerator<T> tEnum)
                {
                    if (tEnum.MoveNext())
                    {
                        var tValue = tEnum.Current;
                        yield return tValue;
                    }
                    else
                    {
                        stack.Pop();
                        tEnum.Dispose();
                    }
                }
                else
                {
                    // unrecognized item on top of stack.. get rid of it
                    stack.Pop();
                }
            }
        }

        public static Chunk<T> ToValueChunk<T>(this T item) =>
            new Chunk<T>(item);

        public static Chunk<T> ToSequenceChunk<T>(this IEnumerable<T> items) =>
            new Chunk<T>(items);

        public static Chunk<T> ToSequenceChunk<T>(this IEnumerable<Chunk<T>> items) =>
            new Chunk<T>(items);
    }
}

