# HuffmanCoder
Encode and decode huffman messages padded to 1460 bytes

# Building

cd into the same directory as the .csproj file and run one of the following:

`dotnet publish -r ubuntu.20.04-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true --self-contained` for linux

`dotnet publish -r win10-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true --self-contained` for windows

This will create `bin/Release/netcoreapp3.1/<either win10-x64 or ubuntu.20.04-x64>/publish` which will contain the executable which can then be run using the command line (`HuffmanCoder.exe` on windows and `./HuffmanCoder` on linux)

# Usage

`HuffmanCoder <read/write> <in file> <out file>` is the basic syntax. If reading, `stdout` is allowed for outfile

## Make sure you have a tree.json file in the same directory when reading 

If you want to read a file called cyber1: `HuffmanCoder read cyber1 stdout` will read the contents

If you want to write the contents of bigfile.txt into cyber1: `HuffmanCoder write bigfile.txt cyber1`
