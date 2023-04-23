Similar to CVE-2023-28260, this is a DLL injection in programs created using
.NET NativeAOT.

This was tested with .NET 7.0.5.

`run.cmd` builds and runs the proof of concept.

Expected output:

```
c:\Windows\System32\bcrypt.dll
```

Actual output:

```
C:\temp\poc\bin\release\net7.0\win-x64\publish\bcrypt.dll
```

# How to fix

Either remove all DLLs not in KnownDLLs from
https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/BuildIntegration/WindowsAPIs.txt
or add this to
https://github.com/dotnet/runtime/blob/main/src/coreclr/nativeaot/BuildIntegration/Microsoft.NETCore.Native.Windows.targets

```
<ItemGroup>
    <LinkerArg Include="/DEPENDENTLOADFLAG:0x800" />
</ItemGroup>
```

An example of this approach is in `poc.csproj`.
