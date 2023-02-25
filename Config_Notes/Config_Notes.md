# DDD-group-project
> DDD group config notes

- [DDD-group-project](#ddd-group-project)
  - [NuGet](#NuGet)

## NuGet

- The nuget configuration might need tweaking on your vs2022 installation

➜  ~ git:(main) ✗ cd /mnt/c/Program\ Files\ \(x86\)/NuGet/Config
➜  Config ls
Microsoft.VisualStudio.FallbackLocation.config  Microsoft.VisualStudio.Offline.config
➜  Config cat Microsoft.VisualStudio.Offline.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="Microsoft Visual Studio Offline Packages" value="C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\"/>
    <add key="Enterprise NuGet Packages" value="https://enterprise.local:44315/nuget" />
    <add key="NuGet Packages" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
➜  Config
