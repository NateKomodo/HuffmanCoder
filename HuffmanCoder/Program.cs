using System;
using System.Collections;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace HuffmanCoder
{
    /*
     * File schema:
     * first 4 bytes: tree length declaration
     * next 4 bytes: body length declaration
     * <tree>
     * <body>
     */
    public static class Program
    {
        private static readonly string _helpMsg = "Please use: HuffmanCoder <read/write> <in file> <out file>. " +
                                                  "Out file can be stdout to write to console";
        
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine(_helpMsg);
                return;
            }
            switch (args[0])
            {
                case "write":
                    Write(File.ReadAllText(args[1]), args[2]);
                    break;
                case "read":
                    Read(args[1], args[2]);
                    break;
                default:
                    Console.WriteLine(_helpMsg);
                    break;
            }
        }

        private static void Write(string input, string path)
        {
            var huffmanTree = new Tree();
            
            huffmanTree.Build(input);
            
            var encoded = huffmanTree.Encode(input);

            var decoded = huffmanTree.Decode(encoded);

            if (input == decoded)
            {
                Console.WriteLine("Encode OK");
            }
            else
            {
                Console.WriteLine("Encode failed, mismatch");
                return;
            }

            Console.WriteLine($"Huffman size: {encoded.Count} bits, ascii size: {input?.Length * 8} bits, savings is {(input?.Length * 8) - encoded.Count} bits");

            var jsonTree = JsonConvert.SerializeObject(huffmanTree.Root);

            var bodyLengthDeclaration = BitConverter.GetBytes((int)Math.Ceiling(encoded.Length / 8f));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bodyLengthDeclaration);

            var encodedBytes = new byte[(int)Math.Ceiling(encoded.Length / 8f)];
            encoded.CopyTo(encodedBytes, 0);
            
            using (var stream = new FileStream(path, FileMode.Create)) {
                stream.Write(bodyLengthDeclaration, 0, bodyLengthDeclaration.Length);
                stream.Write(encodedBytes, 0, encodedBytes.Length);
                
                var bytes = bodyLengthDeclaration.Length + encodedBytes.Length;
                var padding = 1460 - bytes;
                if (padding < 0)
                {
                    Console.WriteLine("Failed, too many characters");
                    return;
                }
                Console.WriteLine($"Wrote {bytes} bytes to {path}, need {padding} padding bytes");

                var paddingBytes = new byte[padding];
                
                stream.Write(paddingBytes, 0, paddingBytes.Length);
                
                Console.WriteLine($"Wrote {paddingBytes.Length} bytes as padding");
            }
            File.WriteAllText("tree.json", jsonTree);
            Console.WriteLine("Updated tree.json");
            
            Console.WriteLine("Done.");
        }

        private static void Read(string infile, string outfile)
        {
            using var stream = new FileStream(infile, FileMode.Open);
            
            var header = new byte[4];
            stream.Read(header, 0, 4);
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(header);
            var len = BitConverter.ToInt32(header, 0);
            
            Console.WriteLine($"Reading {len} bytes");

            var body = new byte[len];
            stream.Read(body, 0, len);

            var bitArray = new BitArray(body);

            var tree = new Tree { Root = JsonConvert.DeserializeObject<Node>(File.ReadAllText("tree.json")) };

            var res = tree.Decode(bitArray);

            if (outfile == "stdout")
            {
                Console.WriteLine(res);
            }
            else
            {
                File.WriteAllText(outfile, res);
            }
            
            Console.WriteLine("Done.");
        }
    }
}