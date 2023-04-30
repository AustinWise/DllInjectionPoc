This is a DLL injection in programs created using .NET when built in singlefile
mode.

This was tested with .NET 7.0.5.

`run.cmd` builds and runs the proof of concept.

Expected output:

```
c:\Windows\System32\version.dll
```

Actual output:

```
C:\temp\poc\bin\release\net7.0\win-x64\publish\version.dll
```

(where the above path is wherever you are currently running this program from)

# How to fix

Apply the delay fix like https://github.com/dotnet/coreclr/pull/24449 to the
single file host in https://github.com/dotnet/runtime/tree/main/src/native/corehost/apphost/static/
