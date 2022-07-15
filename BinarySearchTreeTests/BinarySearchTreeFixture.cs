using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinarySearchTree.Tests
{
    [TestFixture]
    public class BinarySearchTreeFixture
    {
        private BinarySearchTree<int> testBst;
        private List<int> rightResult;

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
            testBst = new BinarySearchTree<int>();
            rightResult = new List<int>();
        }
         
        [TestCase(50)]
        public void TestDeepCopy(int countToAdd)
        {
            FillTheTree(countToAdd);
            BinarySearchTree<int> copiedBst = (BinarySearchTree<int>) testBst.Clone();

            CollectionAssert.AreEqual(copiedBst, testBst);

            int valToDelete = testBst.ElementAt(10);
            copiedBst.Delete(valToDelete);

            CollectionAssert.AreNotEqual(testBst, copiedBst);
            Assert.IsTrue(testBst.Contains(valToDelete));
            Assert.IsFalse(copiedBst.Contains(valToDelete));
        }

        [TestCase(50)]
        public void ConvertTreeToList(int countToAdd)
        {
            FillTheTree(countToAdd);
            List<int> testHashSet = testBst;

            CollectionAssert.AreEquivalent(testHashSet, testBst);
        }

        [TestCase(50)]
        public void ConvertTreeToHashSet(int countToAdd)
        {
            FillTheTree(countToAdd);
            HashSet<int> testHashSet = (HashSet<int>)testBst;

            CollectionAssert.AreEquivalent(testHashSet, testBst);
        }

        [Test]
        public void TreeToString()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(90);
            testBst.Add(50);
            testBst.Add(110);

            Assert.That(testBst.ToString(), Is.EqualTo("20 48 50 90 110"));
        }

        [Test]
        public void ClearEmptyTree_ShoulNotThrowException()
        {
            testBst.Clear();

            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void ClearNotEmptyTree_CountShoulBeZeroAndRootNull()
        {
            FillTheTree(23);
            testBst.Clear();

            Assert.That(testBst.Count, Is.EqualTo(0));
            Assert.That(testBst.Root, Is.EqualTo(null));
        }

        [Test]
        public void EmptyBST_CountShouldBeZero()
        {
            Assert.That(testBst.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddToEmptyBST_ShouldBeAddedtoRoot()
        {
            FillTheTree(1);
            Assert.That(testBst.Root.Value, Is.EqualTo(rightResult[0]));
            Assert.That(testBst.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddToRootRight()
        {
            testBst.Add(100);
            testBst.Add(190);
            Assert.That(testBst.Root.Value, Is.EqualTo(100));
            Assert.That(testBst.Root.Right.Value, Is.EqualTo(190));
        }

        [Test]
        public void AddToRootLeft()
        {
            testBst.Add(100);
            testBst.Add(19);
            Assert.That(testBst.Root.Value, Is.EqualTo(100));
            Assert.That(testBst.Root.Left.Value, Is.EqualTo(19));
        }

        [Test]
        public void EqualValuesShouldBeAdded()
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

        [Test]
        public void PopFromEmptyTree_ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => testBst.Pop());
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
        public void PopNodeWithRightAncestor_RightAncestorShouldBeInThePlaceOfPopped()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(25);

            Assert.That(testBst.Pop(), Is.EqualTo(20));
            Assert.That(testBst.Root.Left.Value, Is.EqualTo(25));
        }

        [Test]
        public void PopNodeWithoutAncestors_AfterPoppedWithoutAncestorsParentShouldBePopped()
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
        public void PopRootWithRightAncestor_RightAncestorShouldBeRoot()
        {
            testBst.Add(48);
            testBst.Add(90);

            Assert.That(testBst.Pop(), Is.EqualTo(48));
            Assert.That(testBst.Root.Value, Is.EqualTo(90));
            Assert.That(testBst.Count, Is.EqualTo(1));
        }

        [Test]
        public void PopRootWithoutAncestors_RootShouldBeNull()
        {
            FillTheTree(1);

            Assert.That(testBst.Pop(), Is.EqualTo(rightResult[0]));
            Assert.That(testBst.Root, Is.EqualTo(null));
        }

        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void DeleteNotExistsValue_ShouldDeleteNothing(int countToAdd)
        {
            FillTheTree(countToAdd);
            int poppedValue = testBst.Pop();
            int countBeforeDeleting = testBst.Count;
            testBst.Delete(poppedValue);
            Assert.That(testBst.Count, Is.EqualTo(countBeforeDeleting));
        }

        [Test]
        public void DeleteValueLeaf_ShouldBeDeletedWithNoRelocations()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(18);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(18);

            Assert.That(testBst.Root.Left.Left, Is.EqualTo(null));
            Assert.That(testBst.Root.Left.Value, Is.EqualTo(20));
            Assert.That(testBst.Count, Is.EqualTo(4));
        }

        [Test]
        public void DeleteValueNodeWithOneAncestor_AncestorShouldBeOnPlaceOfDeleted()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(20);

            Assert.That(testBst.Root.Left.Value, Is.EqualTo(25));
            Assert.That(testBst.Count, Is.EqualTo(3));

            testBst.Delete(25);

            Assert.That(testBst.Root.Left.Value, Is.EqualTo(21));
            Assert.That(testBst.Count, Is.EqualTo(2));
        }

        [Test]
        public void DeleteValueNodeWithTwoAncestors_SmallestLeafValueOfRightAncestorSouldBeOnValueOfDeleted()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(18);
            testBst.Add(25);
            testBst.Add(21);
            testBst.Delete(20);

            Assert.That(testBst.Root.Left.Value, Is.EqualTo(21));
            Assert.That(testBst.Root.Left.Right.Left, Is.EqualTo(null));
        }

        [Test]
        public void DeleteRootWithTwoAncestors_SmallestLeafValueOfRightAncestorShoulBeOnValueOfDeleted()
        {
            testBst.Add(48);
            testBst.Add(20);
            testBst.Add(90);
            testBst.Add(50);
            testBst.Add(110);
            testBst.Delete(48);

            Assert.IsFalse(testBst.Contains(48));
            Assert.That(testBst.Root.Value, Is.EqualTo(50));
        }

        [Test]
        public void DeleteRootWithOneAncestor_AncestorShouldBeARoot()
        {
            testBst.Add(48);
            testBst.Add(90);
            testBst.Add(50);
            testBst.Add(110);
            testBst.Delete(48);

            Assert.IsFalse(testBst.Contains(48));
            Assert.That(testBst.Root.Value, Is.EqualTo(90));
        }

        [Test]
        public void DeleteRootWithoutAncestors_RootShoulBeNull()
        {
            FillTheTree(1);
            testBst.Delete(rightResult[0]);

            Assert.That(testBst.Root, Is.EqualTo(null));
        }

        [TestCase(20)]
        [TestCase(40)]
        [TestCase(100)]
        public void IterateTest_AllShouldBeIterated(int countToAdd)
        {
            FillTheTree(countToAdd);

            for (int i = 0; i < countToAdd; i++)
            {
                Assert.That(testBst.ElementAt(i), Is.EqualTo(rightResult[i]));
            }
        }
    }
}