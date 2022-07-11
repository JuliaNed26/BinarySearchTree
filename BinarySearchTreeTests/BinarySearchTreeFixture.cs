using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySearchTree.Tests
{
    [TestFixture]
    public class BinarySearchTreeFixture
    {
        private BinarySearchTree<int> testBst = new BinarySearchTree<int>();
        private List<int> rightResult = new List<int>();

        public void FillTheTree(int elementsCount)
        {
            Random rand = new Random();
            rightResult = Enumerable.Range(int.MinValue, int.MaxValue)
                          .Select(i => rand.Next())
                          .Distinct()
                          .Take(elementsCount)
                          .Select(i =>
                          {
                              testBst.Add(i);
                              return i;
                          })
                          .OrderBy(i => i)
                          .ToList();
        }

        [SetUp]
        public void RefreshData()
        {
            testBst.Clear();
            rightResult.Clear();
        }

        [Test]
        public void ClearEmptyTree()
        {
            testBst.Clear();

            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void ClearNotEmptyTree()
        {
            FillTheTree(23);
            testBst.Clear();

            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void Count_EmptyBST()
        {
            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddToEmptyBST()
        {
            FillTheTree(1);
            Assert.That(testBst.GetRootValue, Is.EqualTo(rightResult[0]));
            Assert.That(testBst.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddToRootRight()
        {
            testBst.Add(100);
            testBst.Add(190);
            Assert.That(testBst.Count, Is.EqualTo(2));
            Assert.That(testBst.ElementAt<int>(1), Is.EqualTo(190));
        }

        [Test]
        public void AddToRootLeft()
        {
            testBst.Add(100);
            testBst.Add(19);
            Assert.That(testBst.Count, Is.EqualTo(2));
            Assert.That(testBst.ElementAt(0), Is.EqualTo(19));
        }

        [Test]
        public void AddedEqualValues()
        {
            testBst.Add(10);
            testBst.Add(10);
            testBst.Add(9);
            testBst.Add(9);

            Assert.That(testBst.Count, Is.EqualTo(2));
        }

        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void AllAdded(int countToAdd)
        {
            FillTheTree(countToAdd);
            Assert.That(testBst.Count, Is.EqualTo(rightResult.Count));
            CollectionAssert.AreEquivalent(rightResult, testBst.ToList());
        }

        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void PoppedSmallest(int countToAdd)
        {
            FillTheTree(countToAdd);
            Assert.That(testBst.Pop(), Is.EqualTo(rightResult[0]));
            Assert.That(testBst.Count, Is.EqualTo(rightResult.Count - 1));
        }

        [Test]
        public void PopNodeWithRightAncestor()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(25);
            testBst.Pop();

            Assert.That(testBst.Pop(), Is.EqualTo(25));
            Assert.That(testBst.Count, Is.EqualTo(1));
        }

        [Test]
        public void PopNodeWithoutAncestors()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(18);
            testBst.Add(25);

            Assert.That(testBst.Pop(), Is.EqualTo(18));
            Assert.That(testBst.Pop(), Is.EqualTo(20));
            Assert.That(testBst.Count, Is.EqualTo(2));
        }

        [Test]
        public void PopRootWithRightAncestor()
        {
            testBst.Add(48);
            testBst.Add(90);

            Assert.That(testBst.Pop(), Is.EqualTo(48));
            Assert.That(testBst.GetRootValue, Is.EqualTo(90));
            Assert.That(testBst.Count, Is.EqualTo(1));
        }

        [Test]
        public void PopRootWithoutAncestors()
        {
            FillTheTree(1);

            Assert.That(testBst.Pop(), Is.EqualTo(rightResult[0]));
            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteValueLeaf()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(18);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(18);

            Assert.IsFalse(testBst.Contains(18));
        }

        [Test]
        public void DeleteValueNodeWithOneAncestor()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(20);

            Assert.IsFalse(testBst.Contains(20));
            Assert.IsTrue(testBst.Contains(25));

            testBst.Delete(25);

            Assert.IsFalse(testBst.Contains(25));
            Assert.IsTrue(testBst.Contains(21));
        }

        [Test]
        public void DeleteValueNodeWithTwoAncestors()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(18);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(20);

            Assert.IsFalse(testBst.Contains(20));
            Assert.IsTrue(testBst.Contains(25));
            Assert.IsTrue(testBst.Contains(18));
            Assert.IsTrue(testBst.Contains(21));
        }

        [Test]
        public void DeleteRootWithTwoAncestors()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(90);
            testBst.Add(50);
            testBst.Add(110);
            testBst.Delete(48);

            Assert.IsFalse(testBst.Contains(48));
            Assert.That(testBst.GetRootValue, Is.EqualTo(50));
        }

        [Test]
        public void DeleteRootWithOneAncestor()
        {
            testBst.Add(48);
            testBst.Add(90);
            testBst.Add(50);
            testBst.Add(110);
            testBst.Delete(48);

            Assert.IsFalse(testBst.Contains(48));
            Assert.That(testBst.GetRootValue, Is.EqualTo(90));
        }

        [Test]
        public void DeleteRootWithoutAncestors()
        {
            FillTheTree(1);
            testBst.Delete(rightResult[0]);

            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void IterateTest(int countToAdd)
        {
            FillTheTree(countToAdd);

            for (int i = 0; i < countToAdd; i++)
            {
                Assert.That(testBst.ElementAt(i), Is.EqualTo(rightResult[i]));
            }
        }
    }
}