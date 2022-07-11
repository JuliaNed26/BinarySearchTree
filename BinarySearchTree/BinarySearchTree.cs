using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public class BinarySearchTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public BinarySearchTree()
        {
            Count = 0;
        }
        public Node Root { get; private set; }
        public int Count { get; private set; }

        public void Add(T value)
        {
            if (Root == null)//if we don't have root
            {
                Root = new Node(value);
                Count++;
                return;
            }

            Add(Root);

            void Add(Node curRoot)
            {
                bool insertToLeft = curRoot.value.GreaterThan<T>(value) && curRoot.left == null;
                bool insertToRight = curRoot.value.LessThan<T>(value) && curRoot.right == null;

                if (curRoot.value.Equals(value))
                {
                    return;
                }
                if (insertToLeft)
                {
                    curRoot.left = new Node(value);
                    Count++;
                    return;
                }
                if(insertToRight)
                {
                    curRoot.right = new Node(value);
                    Count++;
                    return;
                }
                if(curRoot.value.GreaterThan<T>(value))
                {
                    Add(curRoot.left);
                }
                else
                {
                    Add(curRoot.right);
                }
            }
        }
        public T Pop()
        {
            if(Root == null)
            {
                throw new InvalidOperationException("Could not pop from an empty tree");
            }
            T valueDeleted;
            if (Root.left == null)
            {
                valueDeleted = Root.value;
                Root = Root.right;
            }
            else
            {
                valueDeleted = Pop(Root, Root.left);
            }
            Count--;
            return valueDeleted;

            T Pop(Node parent, Node current)
            {
                if (current.left == null)
                {
                    parent.left = current.right;
                    return current.value;
                }
                else
                {
                    return Pop(current, current.left);
                }
            }
        }
        public void Delete(T value)
        {
            try
            {
                DeleteNodeWithValue(value, null, Root);
                Count--;
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Value does not exists");
            }

            void DeleteNodeWithValue(T value, Node parent, Node curNode)
            {
                if (value.Equals(curNode.value))
                {
                    DeleteNode(parent, curNode);
                    return;
                }
                else if (value.GreaterThan<T>(curNode.value) && curNode.right != null)
                {
                    DeleteNodeWithValue(value, curNode, curNode.right);
                }

                else if (value.LessThan<T>(curNode.value) && curNode.left != null)
                {
                    DeleteNodeWithValue(value, curNode, curNode.left);
                }
                else
                {
                    throw new InvalidDataException();
                }
            }

            void DeleteNode(Node parent, Node nodeToDel)
            {
                if (nodeToDel.right != null && nodeToDel.left != null)
                {
                    nodeToDel.value = FindAndPopSmallestLeaf(nodeToDel, nodeToDel.right);
                }
                else if (nodeToDel.left == null && nodeToDel.right == null)
                {
                    FindAndPopSmallestLeaf(parent, nodeToDel);
                }
                else
                {
                    if(parent == null)//root
                    {
                        Root = nodeToDel.left != null ? nodeToDel.left : nodeToDel.right;
                    }
                    else if (parent.left != null && parent.left.value.Equals(nodeToDel.value))
                    {
                        parent.left = nodeToDel.left != null ? nodeToDel.left : nodeToDel.right;
                    }
                    else
                    {
                        parent.right = nodeToDel.left != null ? nodeToDel.left : nodeToDel.right;
                    }
                }
            }

            T FindAndPopSmallestLeaf(Node parent, Node curNode)
            {
                if (curNode.left != null)
                {
                    return FindAndPopSmallestLeaf(curNode, curNode.left);
                }
                else if (curNode.right != null)
                {
                    return FindAndPopSmallestLeaf(curNode, curNode.right);
                }
                else
                {
                    var leafVal = curNode.value;
                    if (parent == null)//root
                    {
                        Root = null;
                    }
                    else if (parent.left != null && parent.left.value.Equals(curNode.value))
                    {
                        parent.left = null;
                    }
                    else
                    {
                        parent.right = null;
                    }

                    return leafVal;
                }
            }
        }

        public void Clear()
        {
            while(Root != null)
            {
                Pop();
            }
        }

        public bool Contains(T value)
        {
            Node curNode = Root;
            while(curNode != null)
            {
                if(curNode.value.Equals(value))
                {
                    return true;
                }
                curNode = curNode.value.GreaterThan<T>(value) ? curNode.left : curNode.right;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator() => Iterate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> Iterate()
        {
            IEnumerable<T> IterateRecursively(Node curNode)
            {
                if (curNode.left != null)
                {
                    foreach (var item in IterateRecursively(curNode.left))
                    {
                        yield return item;
                    }
                }
                yield return curNode.value;
                if (curNode.right != null)
                {
                    foreach (var item in IterateRecursively(curNode.right))
                    {
                        yield return item;
                    }
                }
            }
            foreach (var item in IterateRecursively(Root))
            {
                yield return item;
            }
        }

        public class Node
        {
            public T value;
            public Node left;
            public Node right;

            public Node(T val) => value = val;
        }

        //public IEnumerator<T> GetEnumerator() => new BSTEnumerator(this);
        //
        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //private class BSTEnumerator : IEnumerator<T>
        //{
        //    private BinarySearchTree<T> _bst;
        //    private Stack<Node> _readNodes;
        //    private Node _current;
        //    private bool _toRight = false;
        //    public BSTEnumerator(BinarySearchTree<T> _bst_)
        //    {
        //        _bst = _bst_;
        //        _readNodes = new Stack<Node>();
        //        _current = _bst._root;
        //    }
        //
        //    public T Current => _current.value;
        //
        //    object IEnumerator.Current => Current;
        //
        //    public void Dispose()
        //    {
        //    }
        //
        //    public bool MoveNext()
        //    {
        //        if (_toRight)
        //        {
        //            _current = _current.right;
        //        }
        //        while (true)
        //        {
        //            if (_current != null)
        //            {
        //                _readNodes.Push(_current);
        //                _current = _current.left;
        //            }
        //            else if (_readNodes.Count != 0)
        //            {
        //                _current = _readNodes.Pop();
        //                _toRight = true;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //
        //    public void Reset()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

    }

    static class ComparisonOperations
    {
        public static bool GreaterThan<T>(this T valueToCheck, T item2) where T : IComparable<T>
            => valueToCheck.CompareTo(item2) > 0;
        public static bool Equals<T>(T item1, T item2) where T : IComparable<T>
            => item1.CompareTo(item2) == 0;
        public static bool LessThan<T>(this T valueToCheck, T item2) where T : IComparable<T>
            => valueToCheck.CompareTo(item2) < 0;
    }
}

