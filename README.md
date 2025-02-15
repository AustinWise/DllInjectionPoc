There is a DLL injection vulnerability in programs using the msquic.dll library.
The msquic library is developed in the open at https://github.com/microsoft/msquic .

The problem is that if an attacker is able to place a DLL with the name `WINMM.dll`
in the same directory as `msquic.dll`, it would be loaded instead
of the correct one System32.

I have previously filed a case about this ( VULN-102108 ) and there was a fix ( https://github.com/microsoft/msquic/pull/3661 ).
However it appears the fix was not effective.
Using the `dumpbin /loadconfig` command, I confirmed that the following versions of msquic do not have
the Dependent Load Flag set:

* 2.3.0
* 2.4.3
* 2.4.7

### POC

This POC is written with .NET 9. I tested with .NET 9.0.2, which includes msquic 2.4.3.

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
