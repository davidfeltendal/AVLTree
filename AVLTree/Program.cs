using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree træ = new AVLTree();
            træ.InsertNode(5);
            træ.Print();
            træ.InsertNode(10); 
            træ.Print();
            træ.InsertNode(7);
            træ.Print();
            træ.InsertNode(12);
            træ.Print();
        }
    }

    class Node
    {
        private int data;
        private int balance;
        private Node left;
        private Node right;
        private Node parrent;

        public Node Left 
        {
            get { return left; }
            set { left = value; }
        }

        public Node Right
        {
            get { return right; }
            set { right = value; }
        }

        public Node Parrent
        {
            get { return parrent; }
            set { parrent = value; }
        }

        public int Data
        {
            get { return data; }
            set { data = value; }
        }

        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public void Print(Node n, string t)
        {
            Node parrentData;
            if (n.parrent != null)
                parrentData = parrent;
            else
                parrentData = new Node();
            t += "-";
            if(right != null)
                right.Print(n.right,t);
            Console.WriteLine(t+" Node: "+n.data+" Parrent: "+parrentData.data+" Balance: "+n.balance);
            if(left != null)
                left.Print(n.left,t);
        }
    }
    class AVLTree
    {
        private Node root;

        public void InsertNode(int data)
        {
            if (root == null)
            {
                root = new Node {Data = data};//bruger get/set properties fra node klassen i stedet for en constructor
                Console.WriteLine("Insert at Root: " + data);
            }
            else
            {
                Node node = root;
                /*denne loop bliver ved med at gå ned i træet 
                 * til den finder den nederste tomme plads*/
                while (node != null)
                {
                    if (data.CompareTo(node.Data) < 0)//hvis indsat er mindre end så indsættes der til venstre
                    {
                        Node left = node.Left;
                        if (left == null) //hvis venstre plads er tom
                        {
                            node.Left = new Node {Data = data, Parrent = node};
                            Console.WriteLine("Insert til venstre efter: " + node.Data + ", værdi: " + data);
                            BalanceNode(node, 1);//balancere op mod højre (+1) venstre side op
                            return;
                        }
                        else
                            node = left;//nuværende node flyttes tn tak ned og hele processen køres igen
                    }
                    else if (data.CompareTo(node.Data) > 0)//ellers insættes til højre
                    {
                        Node right = node.Right;
                        if (right == null)
                        {
                            node.Right = new Node {Data = data, Parrent = node};
                            Console.WriteLine("Insert til højre efter: " + node.Data + ", værdi: " + data);
                            BalanceNode(node, -1); //balancere op mod venstre (-1) højre side op
                            return;
                        }
                        else
                            node = right;
                    }
                    else
                    {
                        node.Data = data;
                        return;
                    }
                }
            }
        }

        public void BalanceNode(Node n, int balance)
        {
            while (n != null)
            {
                balance = (n.Balance += balance);
                if (balance == 0)
                {
                    return;
                }
                else if (balance == 2)//her finder vi ud af om det er en venstreRotation (+ betyder left)
                {
                    if (n.Left.Balance == 1)//her ser vi om der er tale om en single rotation
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("  Print before rotation");
                        Console.WriteLine("--------------------------");
                        Print();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Rotate Right at " + n.Data);
                        RotateRight(n);
                    }
                    else //er det ikke en single rotation er det en double
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("  Print before rotation");
                        Console.WriteLine("--------------------------");
                        Print();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Rotate LeftRight at " + n.Data);
                        RotateLeftRight(n);
                    }
                }
                else if (balance == -2)
                {
                    if (n.Right.Balance == -1)
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("  Print before rotation");
                        Console.WriteLine("--------------------------");
                        Print();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Rotate Left at " + n.Data);
                        RotateLeft(n);
                    }
                    else 
                    {
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("  Print before rotation");
                        Console.WriteLine("--------------------------");
                        Print();
                        Console.WriteLine("--------------------------");
                        Console.WriteLine("Rotate RightLeft at " + n.Data);
                        RotateRightLeft(n);
                    }
                    return;
                }
                Node parrent = n.Parrent;
                {
                    if (parrent != null)
                    {
                        if (parrent.Left == n)
                            balance = 1;
                        else
                            balance = -1;
                    }
                    n = parrent;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine("---- Print AVL Træ ----");
            string t = "";
            if (root != null)
            {
                root.Print(root,t);
            }
            Console.WriteLine("-----------------------");
            Console.WriteLine();
        }
        #region rotation left, right, leftright og rightleft
        #region left

        /*

                            ROTATION VENSTRE         
                               (5)                   (4)
                              /   \                 /   \
                             (4)   S               /     \
                            /   \        -->     (3)     (5)
                           (3)   R               / \     / \
                          /   \                 /   \   /   \
                         P     Q               P     Q R     S

        */



        //Denne Del af kode er ikke del af Pensum.
        private Node RotateLeft(Node node)
        {

            Node right = node.Right;//gemmer (4)
            Node rightLeft = right.Left;//gemmer (R)
            Node parrent = node.Parrent;//gemmer (5) 

            right.Parrent = parrent;//sætter (5) til (4)
            right.Left = node;//sætter (5) til (4)'s left
            node.Right = rightLeft;//sætter (R) til (5)'s right
            node.Parrent = right;//sætter (4) til root/parrent

            if (rightLeft != null)
            {
                rightLeft.Parrent = node;//gemmer (4) som root
            }
            if (node == root)
            {
                root = right;
            }
            else if (parrent.Right == node)
            {
                parrent.Right = right;//flytter (4) op til parrent
            }
            else
            {
                parrent.Left = right;//flytter (4) op til parrent
            }
            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }
        #endregion
        #region right

        /*

                            ROTATION HØJRE        
                             (3)                     (4)
                            /   \                   /   \
                           P   (4)                 /     \
                              /   \      -->     (3)     (5)
                             Q   (5)             / \     / \
                                /   \           /   \   /   \
                               R     S         P     Q R     S

        */

        private Node RotateRight(Node node)
        {

            Node left = node.Left;
            Node leftRight = left.Right;
            Node parrent = node.Parrent;

            left.Parrent = parrent;
            left.Right = node;
            node.Left = leftRight;
            node.Parrent = left;

            if (leftRight != null)
            {
                leftRight.Parrent = node;
            }
            if (node == root)
            {
                root = left;
            }
            else if (parrent.Left == node)
            {
                parrent.Left = left;
            }
            else
            {
                parrent.Right = left;
            }
            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }
        #endregion
        #region left>right
        /*
                             ROTATION VENSTRE OG SÅ HØJRE        
                       (5)                  (5)                 (4)
                      /   \                /   \               /   \
                     (3)   S              (4)   S             /     \
                    /   \        -->     /   \       -->    (3)     (5)
                   P    (4)             (3)   R             / \     / \
                       /   \           /   \               /   \   /   \
                      Q     R         P     Q             P     Q R     S     

        */
        private Node RotateLeftRight(Node node)
        {
            Node left = node.Left;
            Node leftRight = left.Right;
            Node parrent = node.Parrent;
            Node leftRightRight = leftRight.Right;
            Node leftRightLeft = leftRight.Left;

            leftRight.Parrent = parrent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parrent = leftRight;
            node.Parrent = leftRight;
            
            if (leftRightRight != null)
            {
                leftRightRight.Parrent = node;
            }
            if (leftRightLeft != null)
            {
                leftRightLeft.Parrent = left;
            }
            if (node == root)
            {
                root = leftRight;
            }
            else if (parrent.Left == node)
            {
                parrent.Left = leftRight;
            }
            else
            {
                parrent.Right = leftRight;
            }
            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }
            leftRight.Balance = 0;
            return leftRight;
        }
        #endregion
        #region right>left
        /*
                      ROTATION HØJRE OG SÅ VENSTRE
                     (3)                 (3)                     (4)
                    /   \               /   \                   /   \
                   P   (5)             P   (4)                 /     \
                      /   \      -->      /   \      -->     (3)     (5)
                    (4)    D             Q   (5)             / \     / \
                   /   \                    /   \           /   \   /   \
                  B     C                  R     S         P     Q R     S
         */
        private Node RotateRightLeft(Node node)
        {

            Node right = node.Right;
            Node rightLeft = right.Left;
            Node parent = node.Parrent;
            Node rightLeftLeft = rightLeft.Left;
            Node rightLeftRight = rightLeft.Right;

            rightLeft.Parrent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parrent = rightLeft;
            node.Parrent = rightLeft;
            if (rightLeftLeft != null)
            {
                rightLeftLeft.Parrent = node;
            }
            if (rightLeftRight != null)
            {
                rightLeftRight.Parrent = right;
            }
            if (node == root)
            {
                root = rightLeft;
            }
            else if (parent.Right == node)
            {
                parent.Right = rightLeft;
            }
            else
            {
                parent.Left = rightLeft;
            }
            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }
            rightLeft.Balance = 0;
            return rightLeft;
        }
        #endregion
        #endregion
    }

}
