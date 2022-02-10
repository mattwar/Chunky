# Chunky
Chunky is a small API library that can be used to convert quadratically scaling recursive iterators into linear ones.


Instead of writing a non-linear iterator like this:

```csharp
    public class Node
    {
        public int Id { get; }
        public Node? Left { get; }
        public Node? Right { get; }

        public Node(int id, Node? left, Node? right)
        {
            this.Id = id;
            this.Left = left;
            this.Right = right;
        }

        public IEnumerable<Node> DescendantsAndSelf
        {
            get
            {
                if (this.Left != null)
                {
                    foreach (var ln in this.Left.DescendantsAndSelf)
                        yield return ln;
                }

                yield return this;

                if (this.Right != null)
                {
                    foreach (var rn in this.Right.DescenantsAndSelf)
                        yield return rn;
                }
            }
        }
    }
```

write a private iterator like this that iterates chunks and then expose a flattened version of it.

```csharp
    using Chunky;

    public class Node
    {
        public int Id { get; }
        public Node? Left { get; }
        public Node? Right { get; }

        public Node(int id, Node? left, Node? right)
        {
            this.Id = id;
            this.Left = left;
            this.Right = right;
        }

        public IEnumerable<Node> DescendantsAndSelf =>
            new FlattenedEnumerable<Node>(this.ChunkyDescendantsAndSelf);

        private IEnumerable<Chunk<Node>> ChunkyDescendantsAndSelf
        {
            get
            {
                if (this.Left != null)
                    yield return this.Left.DescendantsAndSelf.ToSequenceChunk();

                yield return this.ToValueChunk();

                if (this.Right != null)
                    yield return this.Right.DescendantsAndSelf.ToSequenceChunk();
            }
        }
    }
```
