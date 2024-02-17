Similar to CVE-2023-28260, this is a DLL injection in programs created using
.NET NativeAOT.

This was tested with .NET 8.0.2.

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

An example of the second approach is in `poc.csproj`.

# MSRC response

I ran my proposed fix past the Microsoft Security Response Center before posting this publicly.
They said it would be OK to post to the public bug tracker:

> This case is “Scenario 2: Malicious binary planted in an untrusted application directory.” from https://msrc.microsoft.com/blog/2018/04/triaging-a-dll-planting-vulnerability/ . According to this guidance, a DLL planting issue that falls into this category is treated as Defense-in-Depth issue that will be considered for updates in future versions only. We resolve any MSRC case that fall in this category as vNext consideration.

> The native AOT behavior has been documented at https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/interop#direct-pinvoke-calls and it matches the default behavior of other native programming environments such as Microsoft C/C++.

> You can go ahead and submit a pull request with the proposed patch. The discussion about this issue and the proposed patch can be held in public, given that this is a defense-in-depth issue for vNext consideration only.
