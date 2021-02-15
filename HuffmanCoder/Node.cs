using System.Collections.Generic;

namespace HuffmanCoder
{
    public sealed class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                return symbol.Equals(Symbol) ? data : null;
            }

            List<bool> left = null;

            if (Left != null)
            {
                var leftPath = new List<bool>();
                leftPath.AddRange(data);
                leftPath.Add(false);

                left = Left.Traverse(symbol, leftPath);
            }

            if (Right == null) return left;
                
            var rightPath = new List<bool>();
            rightPath.AddRange(data);
            rightPath.Add(true);
            var right = Right.Traverse(symbol, rightPath);

            return left ?? right;
        }
    }
}