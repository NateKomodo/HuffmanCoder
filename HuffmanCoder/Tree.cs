using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuffmanCoder
{
    public sealed class Tree
    {
        public Node Root { get; set; }

        public void Build(string source)
        {
            var frequencies = new Dictionary<char, int>();
            
            foreach (var c in source)
            {
                if (!frequencies.ContainsKey(c))
                {
                    frequencies.Add(c, 0);
                }

                frequencies[c]++;
            }
            
            if (frequencies.Keys.Count < 2) throw new ArgumentOutOfRangeException(nameof(source), "At least 2 unique characters are required");

            var nodes = new List<Node>();
            
            foreach (var (c, i) in frequencies)
                nodes.Add(new Node() { Symbol = c, Frequency = i });

            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    var taken = orderedNodes.Take(2).ToList();
                    
                    var parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }
                Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(string source)
        {
            var encodedSource = new List<bool>();

            foreach (var encodedSymbol in source.Select(c => Root.Traverse(c, new List<bool>())))
                encodedSource.AddRange(encodedSymbol);

            var bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            var current = Root;
            var decoded = "";

            foreach (bool bit in bits)
            {
                if (bit) 
                    current = current.Right ?? current;
                else
                    current = current.Left ?? current;

                if (current.Left != null || current.Right != null) continue;
                decoded += current.Symbol;
                current = Root;
            }

            return decoded;
        }
    }
}