using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chunky;

namespace ChunkyTests
{
    [TestClass]
    public class EmployeeTests
    {
        public class Employee
        {
            public int Id { get; }
            public IEnumerable<Employee> DirectReports { get; }

            public Employee(int id, params Employee[] directReports)
            {
                this.Id = id;
                this.DirectReports = directReports;
            }

            public IEnumerable<Employee> AllReports =>
                new FlattenedEnumerable<Employee>(this.ChunkyAllReports);

            private IEnumerable<Chunk<Employee>> ChunkyAllReports
            {
                get
                {
                    foreach (var report in this.DirectReports)
                    {
                        yield return report.ToValueChunk();
                        yield return report.AllReports.ToSequenceChunk();
                    }
                }
            }
        }

        [TestMethod]
        public void TestAllReports()
        {
            TestAllReports(0, _ => 5);
            TestAllReports(1, _ => 5);
            TestAllReports(2, _ => 5);
            TestAllReports(3, _ => 5);
            TestAllReports(4, _ => 5);
            TestAllReports(5, _ => 5);
        }

        private static void TestAllReports(int maxDepth, Func<int, int> fnReportsAtDepth)
        {
            var (headHoncho, expectedAllReportsCount) = GenerateEmployee(maxDepth, fnReportsAtDepth);
            TestAllReports(headHoncho, expectedAllReportsCount);
        }

        private static void TestAllReports(Employee headHoncho, int expectedAllReportsCount)
        {
            var allReports = headHoncho.AllReports.ToList();
            Assert.AreEqual(expectedAllReportsCount, allReports.Count, "expected all reports count");
        }

        private static (Employee headHoncho, int allReportsCount) GenerateEmployee(int maxDepth, Func<int, int> fnReportsAtDepth)
        {
            int nextId = 0;
            return (Generate(0), nextId - 1);

            Employee Generate(int depth)
            {
                var id = nextId++;
                var reports = Enumerable.Range(0, depth < maxDepth ? fnReportsAtDepth(depth) : 0).Select(i => Generate(depth + 1)).ToArray();
                return new Employee(id, reports);
            }
        }
    }
}