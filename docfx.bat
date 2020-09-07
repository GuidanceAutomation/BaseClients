@ECHO OFF
ECHO Building docfx project started...
START "" /wait "%~dp0\lib\docfx\docfx.exe" "%~dp0\docfx_project\docfx.json"
ECHO Building docfx project complete
PAUSE
