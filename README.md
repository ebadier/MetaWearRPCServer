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
### Dependencies
* TheNetTunnel (https://github.com/tmteam/TheNetTunnel)

## MetaWearRPC.Net4.6 
Use this library to create RPC Clients to send commands to the MetaWear's RPC Server.
The dlls can be used in Unity3D (version >= 2017). See the Unity3D RPC Client project : https://github.com/ebadier/MetaWearUnityRPC
### Configure Unity3D to use the dlls:
* Go to Edit > ProjectSettings > Player:
  * Scripting Runtime Version : Experimental(.Net 4.6 Equivalent)
  * Scripting Backend : Mono
  * Api Compatibility Level : .Net 4.6
* Copy MetaWearRPC.Net4.6.dll, protobuf-net.dll, TNT.dll in Assets folder.
### Dependencies
* TheNetTunnel (https://github.com/tmteam/TheNetTunnel)
* MetaWear-SDK-CSharp (https://github.com/mbientlab/MetaWear-SDK-CSharp)
* MetaWear-SDK-CSharp-Plugin-Win10 (https://github.com/mbientlab/MetaWear-SDK-CSharp-Plugin-Win10)
