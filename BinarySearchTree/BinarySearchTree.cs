using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        protected class Node
        {
            public T value;
            public Node left;
            public Node right;

            public Node(T val) => value = val;
        }

        private Node _root;

        public void Insert(T value)
        {
            if (_root == null)
            {
                _root = new Node(value);
            }
            else
            {
                Node FindParentRoot(T value, Node curRoot)
                //if it's a new value returns node witch can be a parent
                //if no, returns a parent
                {
                    if (curRoot.value.GreaterThan(value))
                    {
                        return curRoot.left == null || curRoot.left.value.CompareTo(value) == 0
                            ? curRoot : FindParentRoot(value, curRoot.left);
                    }

                    return curRoot.right == null || curRoot.right.value.CompareTo(value) == 0
                        ? curRoot : FindParentRoot(value, curRoot.right);
                }

                Node insertLeaf = FindParentRoot(value, _root);
                if (!insertLeaf.value.Equals(value))
                {
                    if (insertLeaf.value.GreaterThan(value))
                    {
                        insertLeaf.left = new Node(value);
                    }
                    else
                    {
                        insertLeaf.right = new Node(value);
                    }
                }
            }
        }
        public T Pop()
        {
            T Pop(Node parent, Node current)
            {
                if (current.left == null)
                {
                    parent.left = current.right != null ? current.right : null;
                    return current.value;
                }
                else
                {
                    return Pop(current, current.left);
                }
            }

            T valueDeleted;
            if (_root.left == null)
            {
                valueDeleted = _root.value;
                _root = _root.right != null ? _root.right : null;
            }
            else
            {
                valueDeleted = Pop(_root, _root.left);
            }
            return valueDeleted;
        }
        private T PopLeaf(Node parent, Node curNode)
        {
            if (curNode.left != null)
            {
                return PopLeaf(curNode, curNode.left);
            }
            else if (curNode.right != null)
            {
                return PopLeaf(curNode, curNode.right);
            }
            else
            {
                var leafVal = curNode.value;
                if (parent.left != null && parent.left.value.Equals(curNode.value))
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

        private void DeleteNode(Node parent, Node nodeToDel)
        {
            if (nodeToDel.right != null && nodeToDel.left != null)
            {
                nodeToDel.value = PopLeaf(nodeToDel, nodeToDel.right);
            }
            else if (nodeToDel.left == null && nodeToDel.right == null)
            {
                PopLeaf(parent, nodeToDel);
            }
            else
            {
                if (parent.left != null && parent.left.value.Equals(nodeToDel.value))
                {
                    parent.left = nodeToDel.left != null ? nodeToDel.left : nodeToDel.right;
                }
                else
                {
                    parent.right = nodeToDel.left != null ? nodeToDel.left : nodeToDel.right;
                }
            }
        }
        private void DeleteNodeWithValue(T value, Node parent, Node curNode)
        {
            if (value.Equals(curNode.value))
            {
                DeleteNode(parent, curNode);
                return;
            }
            else if (value.GreaterThan(curNode.value) && curNode.right != null)
            {
                DeleteNodeWithValue(value, curNode, curNode.right);
            }

            else if (!value.GreaterThan(curNode.value) && curNode.left != null)
            {
                DeleteNodeWithValue(value, curNode, curNode.left);
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        public void Delete(T value)
        {
            try
            {
                DeleteNodeWithValue(value, null, _root);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Value does not exists");
            }
        }

        //public IEnumerator<T> GetEnumerator() => new BSTEnumerator(this);
        //
        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> Iterate()
        {
            IEnumerable<T> IterateRecursively(Node curNode)
            {
                if (curNode.left != null)
                {
                    foreach(var item in IterateRecursively(curNode.left))
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
            foreach (var item in IterateRecursively(_root))
            {
                yield return item;
            }
        }



        //protected class BSTEnumerator : IEnumerator<T>
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
        public static bool GreaterThan<T>(this T item1, T item2) where T : IComparable<T>
            => item1.CompareTo(item2) > 0;
        public static bool Equals<T>(this T item1, T item2) where T : IComparable<T>
            => item1.CompareTo(item2) == 0;

    }
}

