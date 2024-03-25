rem https://stackoverflow.com/a/1645992
rem @echo off
rem echo %%~dp0 is "%~dp0"
copy /y/b "Assets\README.md" "..\README.md"
findstr "<AssemblyVersion>" "TranslateCS2.csproj" > "..\latest"