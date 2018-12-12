# MetaWear RPC Server C#
Regarding the difficulties to integrate the official MetaWear-SDK-CSharp (https://github.com/mbientlab/MetaWear-SDK-CSharp) into Unity3D,
this solution provides the ability to send commands to MetaWear boards to non-supported platforms (e.g Unity3D) via RPC.

# Projects
- A MetaWear's RPC Server (project MetaWearRPC_Server.Net4.6): this application connects to some MetaWear boards (specified in the file mwBoards.cfg).
It ensures the MetaWear boards stay always connected via Bluetooth LE.
Some RPC Clients can connect to this server to send commands (see IMetaWearContract.cs) via RPC to the MetaWear boards.

- A MetaWear RPC Library (project MetaWearRPC.Net4.6) : use it to create RPC Clients to send commands to the MetaWear's RPC Server.
The dll can be used in Unity3D since version 2017.

# Dependencies
- MetaWearRPC.Net4.6 : TheNetTunnel (https://github.com/tmteam/TheNetTunnel)
- MetaWearRPC_Server.Net4.6 : TheNetTunnel (https://github.com/tmteam/TheNetTunnel), 
MetaWear-SDK-CSharp (https://github.com/mbientlab/MetaWear-SDK-CSharp), MetaWear-SDK-CSharp-Plugin-Win10 (https://github.com/mbientlab/MetaWear-SDK-CSharp-Plugin-Win10)

# Configure Unity3D to use the dlls 
- Go to Edit > ProjectSettings > Player
- Scripting Runtime Version : Experimental(.Net 4.6 Equivalent)
- Scripting Backend : Mono
- Api Compatibility Level : .Net 4.6
- Copy MetaWearRPC.Net4.6.dll, protobuf-net.dll, TNT.dll in Assets folder.