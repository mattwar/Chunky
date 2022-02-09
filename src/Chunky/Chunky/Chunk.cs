using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Chunky
{
    /// <summary>
    /// A piece of a chunky sequence: 
    /// either a singe value, a sequence of values or a sequence of chunks.
    /// </summary>
    public struct Chunk<T>
    {
        private readonly T _value;
        private readonly object? _enum;

        public Chunk(T value)
        {
            _value = value;
            _enum = null;
        }

        public Chunk(IEnumerable<T> sequence)
        {
            _value = default!;
            _enum = sequence is IChunkEnumerable<T> chunky
                ? (object)chunky.GetChunkEnumerator()
                : sequence;
        }

        public Chunk(IEnumerable<Chunk<T>> chunkySequence)
        {
            _value = default!;
            _enum = chunkySequence;
        }

        public Chunk(IEnumerator<Chunk<T>> chunkyEnumerator)
        {
            _value = default!;
            _enum = chunkyEnumerator;
        }

        public static implicit operator Chunk<T>(T value)
        {
            return new Chunk<T>(value);
        }

        public bool TryGetValue([NotNullWhen(true)] out T value)
        {
            value = _value;
            return _enum == null;
        }

        public bool TryGetSequence([NotNullWhen(true)] out IEnumerable<T> sequence)
        {
            if (_enum is IEnumerable<T> ie)
            {
                sequence = ie;
                return true;
            }
            else
            {
                sequence = null!;
                return false;
            }
        }

        public bool TryGetChunkSequence([NotNullWhen(true)] out IEnumerable<Chunk<T>> chunkSequence)
        {
            if (_enum is IEnumerable<Chunk<T>> ie)
            {
                chunkSequence = ie;
                return true;
            }
            else
            {
                chunkSequence = null!;
                return false;
            }
        }

        public bool TryGetChunkEnumerator([NotNullWhen(true)] out IEnumerator<Chunk<T>> chunkEnumerator)
        {
            if (_enum is IEnumerator<Chunk<T>> ie)
            {
                chunkEnumerator = ie;
                return true;
            }
            else
            {
                chunkEnumerator = null!;
                return false;
            }
        }
    }
}