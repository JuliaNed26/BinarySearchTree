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
        private Node _root;
        public BinarySearchTree()
        {
            Count = 0;
        }
        public INode<T> Root => _root;
        public int Count { get; private set; }

        public void Add(T value)
        {
            if (Root == null)//if we don't have root
            {
                _root = new Node(value);
                Count++;
                return;
            }

            Add(_root);

            void Add(Node curRoot)
            {
                bool insertToLeft = curRoot.Value.GreaterThan<T>(value) && curRoot.Left == null;
                bool insertToRight = curRoot.Value.LessThan<T>(value) && curRoot.Right == null;

                if (curRoot.Value.Equals(value))
                {
                    return;
                }
                if (insertToLeft)
                {
                    curRoot.NodeLeft = new Node(value);
                    Count++;
                    return;
                }
                if(insertToRight)
                {
                    curRoot.NodeRight = new Node(value);
                    Count++;
                    return;
                }
                if(curRoot.Value.GreaterThan<T>(value))
                {
                    Add(curRoot.NodeLeft);
                }
                else
                {
                    Add(curRoot.NodeRight);
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
            if (Root.Left == null)
            {
                valueDeleted = Root.Value;
                _root = _root.NodeRight;
            }
            else
            {
                valueDeleted = Pop(_root, _root.NodeLeft);
            }
            Count--;
            return valueDeleted;

            T Pop(Node parent, Node current)
            {
                if (current.Left == null)
                {
                    parent.NodeLeft = current.NodeRight;
                    return current.Value;
                }
                else
                {
                    return Pop(current, current.NodeLeft);
                }
            }
        }
        public void Delete(T value)
        {
            try
            {
                DeleteNodeWithValue(value, null, _root);
                Count--;
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Value does not exists");
            }

            void DeleteNodeWithValue(T value, Node parent, Node curNode)
            {
                if (value.Equals(curNode.Value))
                {
                    DeleteNode(parent, curNode);
                    return;
                }
                else if (value.GreaterThan<T>(curNode.Value) && curNode.Right != null)
                {
                    DeleteNodeWithValue(value, curNode, curNode.NodeRight);
                }

                else if (value.LessThan<T>(curNode.Value) && curNode.Left != null)
                {
                    DeleteNodeWithValue(value, curNode, curNode.NodeLeft);
                }
                else
                {
                    throw new InvalidDataException();
                }
            }

            void DeleteNode(Node parent, Node nodeToDel)
            {
                if (nodeToDel.Right != null && nodeToDel.Left != null)
                {
                    nodeToDel.NodeValue = FindAndPopSmallestLeaf(nodeToDel, nodeToDel.NodeRight);
                }
                else if (nodeToDel.Left == null && nodeToDel.Right == null)
                {
                    FindAndPopSmallestLeaf(parent, nodeToDel);
                }
                else
                {
                    var notNullAncestor = nodeToDel.NodeLeft != null ? nodeToDel.NodeLeft : nodeToDel.NodeRight;

                    if (parent == null)//root
                    {
                        _root = notNullAncestor;
                    }
                    else if (parent.Left != null && parent.Left.Value.Equals(nodeToDel.Value))
                    {
                        parent.NodeLeft = notNullAncestor;
                    }
                    else
                    {
                        parent.NodeRight = notNullAncestor;
                    }
                }
            }

            T FindAndPopSmallestLeaf(Node parent, Node curNode)
            {
                if (curNode.Left != null)
                {
                    return FindAndPopSmallestLeaf(curNode, curNode.NodeLeft);
                }
                else if (curNode.Right != null)
                {
                    return FindAndPopSmallestLeaf(curNode, curNode.NodeRight);
                }
                else
                {
                    var leafVal = curNode.Value;
                    if (parent == null)//root
                    {
                        _root = null;
                    }
                    else if (parent.Left != null && parent.Left.Value.Equals(curNode.Value))
                    {
                        parent.NodeLeft = null;
                    }
                    else
                    {
                        parent.NodeRight = null;
                    }

                    return leafVal;
                }
            }
        }

        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool Contains(T value)
        {
            Node curNode = _root;
            while(curNode != null)
            {
                if(curNode.Value.Equals(value))
                {
                    return true;
                }
                curNode = curNode.NodeValue.GreaterThan<T>(value) ? curNode.NodeLeft : curNode.NodeRight;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator() => Iterate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> Iterate()
        {
            IEnumerable<T> IterateRecursively(Node curNode)
            {
                if (curNode.Left != null)
                {
                    foreach (var item in IterateRecursively(curNode.NodeLeft))
                    {
                        yield return item;
                    }
                }
                yield return curNode.Value;
                if (curNode.Right != null)
                {
                    foreach (var item in IterateRecursively(curNode.NodeRight))
                    {
                        yield return item;
                    }
                }
            }
            foreach (var item in IterateRecursively(_root))
            {
                yield return item;
            }
        }

        private class Node : INode<T>
        {
            public Node(T val) => NodeValue = val;
            public T NodeValue { get; set; }
            public Node NodeLeft { get; set; }
            public Node NodeRight { get; set; }

            public T Value => NodeValue;

            public INode<T> Left => NodeLeft;

            public INode<T> Right => NodeRight;
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

