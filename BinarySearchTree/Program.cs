using BinarySearchTree;

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

var bstList = bst.ToList();

for (int i = 0; i < countToAdd; i++)
{
    Console.WriteLine(allAdded[i] + " " + bstList[i]);
}
