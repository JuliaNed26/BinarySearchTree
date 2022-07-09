using BinarySearchTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BinarySearchTreeTests
{
    [TestClass]
    public class BinarySearchTreeTests
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
            int rightValue = 0;
            var enumerator = bst.GetEnumerator();
            for(int i = 0; i < 2; i++)
            {
                enumerator.MoveNext();
                rightValue = enumerator.Current;
            }
            Assert.AreEqual<int>(20, rightValue);
        }

        [TestMethod]
        public void AddToRootLeft()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(10);
            bst.Add(9);
            Assert.AreEqual<int>(2, bst.Count);
            int leftValue = 0;
            var enumerator = bst.GetEnumerator();
            enumerator.MoveNext();
            leftValue = enumerator.Current;

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
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            HashSet<int> allIteratedNumbers = new HashSet<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);
            bst.Add(21);
            bst.Add(31);
            bst.Add(23);
            bst.Add(21);
            bst.Add(90);
            bst.Add(50);
            bst.Add(110);
            foreach(var item in bst)
            {
                allIteratedNumbers.Add(item);
            }

            Assert.AreEqual<int>(allIteratedNumbers.Count, bst.Count);
        }

        [TestMethod]
        public void PoppedSmallest()
        {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);

            Assert.AreEqual<int>(18, bst.Pop());
            Assert.AreEqual<int>(3, bst.Count);
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
            int[] toCheck = new int[] { 18, 20, 21, 23, 25, 31, 48, 50, 90, 110 };
            bst.Add(48);
            bst.Add(20);
            bst.Add(18);
            bst.Add(25);
            bst.Add(21);
            bst.Add(31);
            bst.Add(23);
            bst.Add(21);
            bst.Add(90);
            bst.Add(50);
            bst.Add(110);

            int iteration = 0;
            foreach(var item in bst)
            {
                Assert.AreEqual<int>(toCheck[iteration], item);
                iteration++;
            }
        }
    }
}