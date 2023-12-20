# dumpall

This is just a simple quick tool to emit some combined dotnet information quickly

```bash
$ dumpall
.NET SDK:
 Version:           8.0.200-preview.23613.10
 Commit:            09b589bed8
 Workload version:  8.0.200-manifests.3b83835e

Runtime Environment:
 OS Name:     Windows
 OS Version:  10.0.22635
 OS Platform: Windows
 RID:         win-x64
 Base Path:   C:\Program Files\dotnet\sdk\8.0.200-preview.23613.10\

.NET workloads installed:
 Workload version: 8.0.200-manifests.3b83835e

 [aspire]
   Installation Source: VS 17.9.34418.183
   Manifest Version:    8.0.0-preview.1.23557.2/8.0.100
   Manifest Path:       C:\Program Files\dotnet\sdk-manifests\8.0.100\microsoft.net.sdk.aspire\8.0.0-preview.1.23557.2\WorkloadManifest.json
   Install Type:        FileBased


Host:
  Version:      8.0.0
  Architecture: x64
  Commit:       5535e31a71

.NET SDKs installed:
  8.0.200-preview.23613.10 [C:\Program Files\dotnet\sdk]

.NET runtimes installed:
  Microsoft.AspNetCore.App 5.0.17 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.AspNetCore.App 6.0.25 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.AspNetCore.App 7.0.14 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.AspNetCore.App 8.0.0 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.NETCore.App 5.0.17 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.NETCore.App 6.0.25 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.NETCore.App 7.0.14 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.NETCore.App 8.0.0 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.WindowsDesktop.App 5.0.17 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
  Microsoft.WindowsDesktop.App 6.0.25 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
  Microsoft.WindowsDesktop.App 7.0.14 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
  Microsoft.WindowsDesktop.App 8.0.0 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]

Azure Developer CLI:
  azd version 1.5.1-daily.3351945 (commit fbbcfbe30f2175d980267329f0c0916c2270d4ed)

Azure CLI:
  {
  "azure-cli": "2.51.0",
  "azure-cli-core": "2.51.0",
  "azure-cli-telemetry": "1.1.0",
  "extensions": {
    "containerapp": "0.3.38"
  }
}

VS Code Extensions
  C# Dev Kit: 1.2.5
  C#: 2.14.8
```

That's it, nothing special. I just wanted a tool to dump a bunch of info quickly.
