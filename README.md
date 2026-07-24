# ClassicUs.ManuAPI.ModExample

A deliberately small Classic Us mod demonstrating the current ManuAPI + Manactor setup.

It registers a virtual Crewmate role called **Ghostling**. The role has a **Cloak** button
that asks the host to make the player invisible for five seconds, then restores the normal
visuals. The example shows virtual roles, `CustomAbility`, Harmony lifecycle patches and a
host-authoritative Manactor RPC without requiring custom assets.

Build with:

```powershell
dotnet build ManuAPI.ModExample.csproj -c Release
```

The project copies `ClassicUs.ManuAPI.ModExample.dll` to the configured Classic Us
`BepInEx/plugins` folder after a successful build.
