# MetaWear RPC Server C#
Regarding the difficulties to integrate the official MetaWear-SDK-CSharp (https://github.com/mbientlab/MetaWear-SDK-CSharp) into Unity3D,
this Windows solution provides the ability to send commands to MetaWear boards to non-supported C# platforms (e.g Unity3D) via RPC.

# Projects
## MetaWearRPC_Server.Net4.6 
This application connects to some MetaWear boards (specified in the file mwBoards.cfg).
It ensures the MetaWear boards stay always connected via Bluetooth LE.
RPC Clients can connect to this server to send commands (see IMetaWearContract.cs) via RPC to the MetaWear boards.
All the features of the MetaWear boards can be easily added by declaring new methods in the IMetaWearContract interface and the corresponding implementations in the MetaWearContract class.

### How to use the RPC Server:
* Fill the file mwBoards.cfg with your MetaWear boards MAC adresses.
* Add the command line argument "..\\..\\..\\mwBoards.cfg" in the Debug section of the project properties.
### WinRT References
* Windows : C:\Program Files (x86)\Windows Kits\10\UnionMetadata\10.0.17134.0\Windows.winmd
### Dependencies (automatically installed using NuGet packages)
* TheNetTunnel (https://github.com/tmteam/TheNetTunnel)
* MetaWear-SDK-CSharp (https://github.com/mbientlab/MetaWear-SDK-CSharp)
* MetaWear-SDK-CSharp-Plugin-Win10 (https://github.com/mbientlab/MetaWear-SDK-CSharp-Plugin-Win10)

## MetaWearRPC.Net4.6 
Use this library to create RPC Clients to send commands to the MetaWear's RPC Server.
The dlls can be used in Unity3D (version >= 2017). See the Unity3D RPC Client project : https://github.com/ebadier/MetaWearUnityRPC
### Configure Unity3D to use the dlls:
* Go to Edit > ProjectSettings > Player:
  * Scripting Runtime Version : Experimental(.Net 4.6 Equivalent)
  * Scripting Backend : Mono
  * Api Compatibility Level : .Net 4.6
* Copy MetaWearRPC.Net4.6.dll, protobuf-net.dll, TNT.dll in Assets folder.
### Dependencies (automatically installed using NuGet packages)
* TheNetTunnel (https://github.com/tmteam/TheNetTunnel)

# License
MIT License

Copyright (c) 2020  
[Emmanuel Badier](mailto:emmanuel.badier@gmail.com)  

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
