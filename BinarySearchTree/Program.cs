using BinarySearchTree;




BinarySearchTree<int> tree = new BinarySearchTree<int>();
try
{
    tree.Pop();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}


