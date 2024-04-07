rem https://stackoverflow.com/a/1645992
@echo off
rem echo %%~dp0 is "%~dp0"
copy /y/b "Assets\README.md" "..\README.md"
echo %1 > "..\latest"