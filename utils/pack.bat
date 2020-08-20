@echo off

mkdir ..\bin\Release\Release

del /S /Q "..\bin\Release\Build.zip"

copy ..\bin\Release\IMG2RBX.exe ..\bin\Release\Release
copy ..\README.md ..\bin\Release\Release

move "..\bin\Release\Release" "\"

tar -a -c -f "..\bin\Release\Build.zip" "\Release"

rd /S /Q "\Release"