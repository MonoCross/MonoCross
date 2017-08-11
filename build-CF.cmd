@echo off
setlocal

echo ** Building MonoCross for .NET Compact Framework v3.5

:: Call MSBuild
echo ** C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  build.xml /p:Configuration=Release;Ext=.CF /v:normal %*
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe  build.xml /p:Configuration=Release;Ext=.CF /v:normal %*
set BUILDERRORLEVEL=%ERRORLEVEL%
echo.

echo ** Build completed. Exit code: %BUILDERRORLEVEL%

exit /b %BUILDERRORLEVEL%

pause
