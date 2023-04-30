@echo off

dotnet publish -r win-x64 -c release
xcopy /y c:\Windows\System32\version.dll bin\release\net7.0\win-x64\publish\
bin\release\net7.0\win-x64\publish\poc.exe