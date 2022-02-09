using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chunky;

namespace ChunkyTests
{
    [TestClass]
    public class BinaryTreeTests
    {
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

        [TestMethod]
        public void TestDescendantsAndSelf()
        {
            TestDescendantsAndSelf(0);
            TestDescendantsAndSelf(1);
            TestDescendantsAndSelf(2);
            TestDescendantsAndSelf(3);
            TestDescendantsAndSelf(4);
            TestDescendantsAndSelf(5);
        }

        private static void TestDescendantsAndSelf(int maxDepth)
        {
            var (root, expectedTotalCount) = GenerateFullTree(maxDepth);
            TestDescendantsAndSelf(root, expectedTotalCount);
        }

        private static void TestDescendantsAndSelf(Node root, int expectedTotalCount)
        {
            var list = root.DescendantsAndSelf.ToList();
            Assert.AreEqual(expectedTotalCount, list.Count, "expected DescendantsAndSelf count");
        }

        private static (Node root, int totalCount) GenerateFullTree(int maxDepth)
        {
            int nextId = 0;
            return (Generate(0), nextId)!;

            Node? Generate(int depth)
            {
                if (depth <= maxDepth)
                {
                    var left = Generate(depth + 1);
                    var id = nextId++;
                    var right = Generate(depth + 1);
                    return new Node(id, left, right);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}