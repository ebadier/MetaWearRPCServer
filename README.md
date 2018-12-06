==> This solution produces Unity3D 2017 compatible dlls (using .Net Framework 4.6 instead of the .NetStandard 2.0) from the official MetaWear SDKs.

> If you would like to update the solution with the latest sources from MbientLab, simply update .cs files in each project:
- project MetaWear.Net must be updated with sources from : https://github.com/mbientlab/MetaWear-SDK-CSharp
- project MetaWear.NetStandard must be updated with sources from : https://github.com/mbientlab/MetaWear-SDK-CSharp-Plugin-NetStandard
- project MetaWear.NetWin10 must be updated with sources from : https://github.com/mbientlab/MetaWear-SDK-CSharp-Plugin-Win10
- project Warble.Net must be updated with sources from : https://github.com/mbientlab/Warble.NET

> Build the dlls for Unity3D:
1) Configure the solution to build in Release/AnyCPU mode.
2) Generate the solution
3) Copy all the dlls and the associated xml files located in MetaWear.NetStandard\bin\Release to an Assets/.../Plugins folder in Unity3D.
*Curently dlls generated with MetaWear.NetWin10 are not usable in Unity3D for desktop applications (Windows/Mac/Linux) but only for UWP apps.
4) Finally, build the project https://github.com/mbientlab/Warble in Release/x64 mode, and copy the dlls to the Assets/.../Plugins folder in Unity3D.
*If building using Visual Studio 2017, git path must be present in Windows Path (typically C:\Program Files\Git\cmd).

> Configure Unity3D to use the dlls (Edit > ProjectSettings > Player):
- Scripting Runtime Version : Experimental(.Net 4.6 Equivalent)
- Scripting Backend : Mono
- Api Compatibility Level : .Net 4.6