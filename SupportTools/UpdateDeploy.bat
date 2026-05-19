@echo off

setlocal

if "%GX_PROGRAM_DIR%" == "" (
    cmd /c echo [91mGX_PROGRAM_DIR is not set. Point it at your GeneXus install directory and try again.[0m
    exit /b 1
)

set Configuration=%1
if %1. == . set Configuration=Debug

xcopy bin\%Configuration%\net471\GeneXus.Packages.SupportTools.* "%GX_PROGRAM_DIR%\Packages\" /d /y
if ERRORLEVEL 1 goto showError

rem call "%GX_PROGRAM_DIR%\Genexus.com" /Install

cmd /c echo [96mSupport Tools binaries updated[0m
goto end

:showError
cmd /c echo [91mFailed copying Support Tools binaries[0m

:end
exit /b %ERRORLEVEL%