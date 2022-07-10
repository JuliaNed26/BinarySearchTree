using BinarySearchTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System;

namespace BinarySearchTreeTests
{
    [TestClass]
    public class BinarySearchTreeFixture

    {
        [TestMethod]
        public void Count_EmptyBST()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            Assert.AreEqual<int>(0, bst.Count);
        }

        [TestMethod]
        public void AddToEmptyBST()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(10);
            Assert.AreEqual<int>(10, bst.GetRootValue);
            Assert.AreEqual<int>(1, bst.Count);
        }

        [TestMethod]
        public void AddToRootRight()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(10);
            bst.Add(20);
            Assert.AreEqual<int>(2, bst.Count);
            int rightValue = bst.ElementAt<int>(1);
            Assert.AreEqual<int>(20, rightValue);
        }

        [TestMethod]
        public void AddToRootLeft()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(10);
            bst.Add(9);
            Assert.AreEqual<int>(2, bst.Count);
            int leftValue = bst.ElementAt(0);

            Assert.AreEqual<int>(9, leftValue);
        }

        [TestMethod]
        public void AddedEqualValues()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(10);
            bst.Add(10);
            bst.Add(9);
            bst.Add(9);

            Assert.AreEqual<int>(2, bst.Count);
        }

        [TestMethod]
        public void AllAdded()
        {
            int countToAdd = 50;
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            Random randNum = new Random();
            var allIteratedNumbers = Enumerable.Range(int.MinValue, int.MaxValue)
                                     .Select(i => randNum.Next())
                                     .Distinct()
                                     .Take(countToAdd)
                                     .Select(i =>
                                    {
                                        bst.Add(i);
                                        return i;
                                    }).ToList();

            Assert.AreEqual<int>(allIteratedNumbers.Count, bst.Count);
            CollectionAssert.AreEquivalent(allIteratedNumbers, bst.ToList());
        }

        [TestMethod]
        public void PoppedSmallest()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            int countToAdd = 50;
            Random rand = new Random();

            List<int> allAddedValues = Enumerable.Range(int.MinValue, int.MaxValue)
                                       .Select(i => rand.Next())
                                       .Distinct()
                                       .Take(countToAdd)
                                       .Select(i =>
                                      {
                                          bst.Add(i);
                                          return i;
                                      })
                                       .OrderBy(i => i)
                                       .ToList();

            Assert.AreEqual<int>(allAddedValues[0], bst.Pop());
            Assert.AreEqual<int>(allAddedValues.Count - 1, bst.Count);
        }

        [TestMethod]
        public void PopNodeWithRightAncestor()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(25);
            bst.Pop();

            Assert.AreEqual<int>(25, bst.Pop());
            Assert.AreEqual<int>(1, bst.Count);
        }

        [TestMethod]
        public void PopNodeWithoutAncestors()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);

            Assert.AreEqual<int>(18, bst.Pop());
            Assert.AreEqual<int>(20, bst.Pop());
            Assert.AreEqual<int>(2, bst.Count);
        }

        [TestMethod]
        public void PopRootWithRightAncestor()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(90);

            Assert.AreEqual<int>(48, bst.Pop());
            Assert.AreEqual<int>(90, bst.GetRootValue);
            Assert.AreEqual<int>(1, bst.Count);
        }

        [TestMethod]
        public void PopRootWithoutAncestors()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);

            Assert.AreEqual<int>(48, bst.Pop());
            Assert.AreEqual<int>(0, bst.Count);
        }

        [TestMethod]
        public void DeleteValueLeaf()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);
            bst.Add(21);
            bst.Delete(18);

            Assert.IsFalse(bst.Contains(18));
        }

        [TestMethod]
        public void DeleteValueNodeWithOneAncestor()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(25);
            bst.Add(21);
            bst.Delete(20);

            Assert.IsFalse(bst.Contains(20));
            Assert.IsTrue(bst.Contains(25));

            bst.Delete(25);

            Assert.IsFalse(bst.Contains(25));
            Assert.IsTrue(bst.Contains(21));
        }

        [TestMethod]
        public void DeleteValueNodeWithTwoAncestors()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);
            bst.Add(21);
            bst.Delete(20);

            Assert.IsFalse(bst.Contains(20));
            Assert.IsTrue(bst.Contains(25));
            Assert.IsTrue(bst.Contains(18));
            Assert.IsTrue(bst.Contains(21));
        }

        [TestMethod]
        public void DeleteRootWithTwoAncestors()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(90);
            bst.Add(50);
            bst.Add(110);
            bst.Delete(48);

            Assert.IsFalse(bst.Contains(48));
            Assert.AreEqual<int>(50, bst.GetRootValue);
        }

        [TestMethod]
        public void DeleteRootWithOneAncestor()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(90);
            bst.Add(50);
            bst.Add(110);
            bst.Delete(48);

            Assert.IsFalse(bst.Contains(48));
            Assert.AreEqual<int>(90, bst.GetRootValue);
        }

        [TestMethod]
        public void DeleteRootWithoutAncestors()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Delete(48);

            Assert.AreEqual<int>(0, bst.Count);
        }

        [TestMethod]
        public void IterateTest()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            int countToAdd = 50;
            Random rand = new Random();
            var allAdded = Enumerable.Range(int.MinValue, int.MaxValue)
                           .Select(i => rand.Next())
                           .Distinct()
                           .Take(countToAdd)
                           .Select(i =>
                          {
                              bst.Add(i);
                              return i;
                          })
                           .OrderBy(i => i)
                           .ToList();

            for(int i = 0; i < countToAdd; i++)
            {
                Assert.AreEqual<int>(allAdded[i], bst.ElementAt(i));
            }
        }
    }
}