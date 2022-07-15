using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public class BinarySearchTree<T> : ICloneable, IEnumerable<T> where T : IComparable<T>
    {
        private Node _root;
        public BinarySearchTree()
        {
            Count = 0;
        }
        public INode<T> Root => _root;
        public int Count { get; private set; }

        public static implicit operator List<T>(BinarySearchTree<T> bst) => bst.Select(item => item).ToList();

        public static explicit operator HashSet<T>(BinarySearchTree<T> bst) => bst.Select(item => item).ToHashSet<T>();

        public override string ToString() => string.Join(" ", this.Select( item => item.ToString()));

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
                    curRoot.Left = new Node(value);
                    Count++;
                    return;
                }
                if(insertToRight)
                {
                    curRoot.Right = new Node(value);
                    Count++;
                    return;
                }
                if(curRoot.Value.GreaterThan<T>(value))
                {
                    Add((Node)curRoot.Left);
                }
                else
                {
                    Add((Node)curRoot.Right);
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
                _root = (Node)_root.Right;
            }
            else
            {
                valueDeleted = Pop(_root, (Node)_root.Left);
            }
            Count--;
            return valueDeleted;

            T Pop(Node parent, Node current)
            {
                if (current.Left == null)
                {
                    parent.Left = current.Right;
                    return current.Value;
                }
                else
                {
                    return Pop(current, (Node)current.Left);
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
                    DeleteNodeWithValue(value, curNode, (Node)curNode.Right);
                }

                else if (value.LessThan<T>(curNode.Value) && curNode.Left != null)
                {
                    DeleteNodeWithValue(value, curNode, (Node)curNode.Left);
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
                    nodeToDel.Value = FindAndPopSmallestLeaf(nodeToDel, (Node)nodeToDel.Right);
                }
                else if (nodeToDel.Left == null && nodeToDel.Right == null)
                {
                    FindAndPopSmallestLeaf(parent, nodeToDel);
                }
                else
                {
                    var notNullAncestor = nodeToDel.Left != null ? nodeToDel.Left : nodeToDel.Right;

                    if (parent == null)//root
                    {
                        _root = (Node)notNullAncestor;
                    }
                    else if (parent.Left != null && parent.Left.Value.Equals(nodeToDel.Value))
                    {
                        parent.Left = notNullAncestor;
                    }
                    else
                    {
                        parent.Right = notNullAncestor;
                    }
                }
            }

            T FindAndPopSmallestLeaf(Node parent, Node curNode)
            {
                if (curNode.Left != null)
                {
                    return FindAndPopSmallestLeaf(curNode, (Node)curNode.Left);
                }
                else if (curNode.Right != null)
                {
                    return FindAndPopSmallestLeaf(curNode, (Node)curNode.Right);
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
                        parent.Left = null;
                    }
                    else
                    {
                        parent.Right = null;
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
                curNode = curNode.Value.GreaterThan<T>(value) ? (Node)curNode.Left : (Node)curNode.Right;
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
                    foreach (var item in IterateRecursively((Node)curNode.Left))
                    {
                        yield return item;
                    }
                }
                yield return curNode.Value;
                if (curNode.Right != null)
                {
                    foreach (var item in IterateRecursively((Node)curNode.Right))
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

        public object Clone()
        {
            BinarySearchTree<T> bst = new BinarySearchTree<T>();

            RecursiveInsertion(_root);

            void RecursiveInsertion(INode<T> node)
            {
                //but we can not check whether T is a reference type, because user can not change the value of a node
                //did it just because this will satisfy deep copy rule
                var valueToCopy = typeof(T).IsValueType ? node.Value : (T)Activator.CreateInstance(typeof(T), node.Value);

                bst.Add(valueToCopy);
                if (node.Right != null)
                {
                    RecursiveInsertion(node.Right);
                }
                if (node.Left != null)
                {
                    RecursiveInsertion(node.Left);
                }
            }
            return bst;
        }

        private class Node : INode<T>
        {
            public Node(T val) => Value = val;
            public Node(T val, Node left, Node right)
            {
                Value = val;
                Left = left;
                Right = right;
            }
            public T Value { get; set; }
            public INode<T> Left { get; set; }
            public INode<T> Right { get; set; }
        }

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

