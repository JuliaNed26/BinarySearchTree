using BinarySearchTree;


BinarySearchTree<int> bst = new BinarySearchTree<int>();
bst.Insert(48);
bst.Insert(20);
bst.Insert(18);
bst.Insert(25);
bst.Insert(21);
bst.Insert(31);
bst.Insert(23);
bst.Insert(90);
bst.Insert(50);
bst.Insert(110);

foreach (var item in bst.Iterate())
{
    Console.WriteLine(item);
}

bst.Delete(25);


foreach (var item in bst.Iterate())
{
    Console.WriteLine(item);
}