@echo off
echo Running all code analysis targets. && echo.
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe build.proj /t:CodeAnalysis /p:Configuration=Debug