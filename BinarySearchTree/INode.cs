using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    public interface INode<T>
    {
            public T Value { get; }
            public INode<T> Left { get; }
            public INode<T> Right { get; }
    }
}
