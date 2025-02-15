@echo off

dotnet publish -r win-x64 -c release poc.csproj
xcopy /y c:\Windows\System32\WINMM.dll bin\release\net9.0\win-x64\publish\
bin\release\net9.0\win-x64\publish\poc.exe
