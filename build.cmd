@echo off
setlocal

:: Check prerequisites
if not "%VisualStudioVersion%" == "14.0" (
    echo Error: build.cmd should be run from a Visual Studio 2015 Command Prompt.  
    exit /b 1
)

:: Check for a custom MSBuild path. If not defined, default to the one in your path.
if not defined MSBUILDCUSTOMPATH (
    set MSBUILDCUSTOMPATH=MSBuild.exe
)

echo ** MSBuild Path: %MSBUILDCUSTOMPATH%
echo ** Building all sources

:: Call MSBuild
echo ** "%MSBUILDCUSTOMPATH%" build.xml /verbosity:normal /p:Configuration=Release
"%MSBUILDCUSTOMPATH%" build.xml /verbosity:normal /p:Configuration=Release
set BUILDERRORLEVEL=%ERRORLEVEL%
echo.

echo ** Build completed. Exit code: %BUILDERRORLEVEL%

exit /b %BUILDERRORLEVEL%

pause