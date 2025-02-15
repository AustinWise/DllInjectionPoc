This is a DLL injection in programs created using .NET 9 under the following
conditions:
* They use the QUIC protocol
* They publish as a self-contained program

The problem is that if an attacker is able to place a DLL with the name `WINMM.dll`
next to the `msquic.dll` deployed as part of .NET 7, it would be loaded instead
of the correct one System32.

The problem is not in how .NET loads this library. The problem is in how MSQuic
references DLLs. This vulnerability applies to any system that uses `msquic.dll`.

This was tested with .NET 9.0.2. The MSQuic version is 2.4.3.

`run.cmd` builds and runs the proof of concept.

Expected output:

```
c:\Windows\System32\WINMM.dll
```

Actual output:

```
C:\temp\poc\bin\release\net9.0\win-x64\publish\WINMM.dll
```

(where the above path is wherever you are currently running this program from)
