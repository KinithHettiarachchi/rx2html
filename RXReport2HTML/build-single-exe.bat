@echo off
REM Build Self-Contained Single EXE for RXReport2HTML

echo Building RXReport2HTML...
echo.

REM Clean previous builds
echo Cleaning previous builds...
dotnet clean -c Release

REM Build and publish as single file
echo Publishing self-contained single executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
    echo.
    echo Output location:
    echo   bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe
    echo.
    echo Usage:
    echo   GUI Mode: Double-click RXReport2HTML.exe
    echo   CLI Mode: RXReport2HTML.exe "path\to\file.rxlog"
) else (
    echo.
    echo Build failed!
    exit /b 1
)

pause
