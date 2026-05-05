@echo off
setlocal enabledelayedexpansion

REM ========================================
REM  RXReport2HTML - Single EXE Builder
REM ========================================

echo.
echo ========================================
echo  Building RXReport2HTML Single EXE
echo ========================================
echo.

REM Navigate to project directory
cd /d "%~dp0RXReport2HTML"

REM Clean previous builds
echo [1/4] Cleaning previous builds...
dotnet clean -c Release >nul 2>&1
if exist "bin\Release" rd /s /q "bin\Release" >nul 2>&1

echo [2/4] Restoring NuGet packages...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Failed to restore packages!
    pause
    exit /b 1
)

echo [3/4] Building and publishing self-contained single executable...
dotnet publish -c Release -r win-x64 --self-contained true ^
    -p:PublishSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:EnableCompressionInSingleFile=true ^
    -p:DebugType=None ^
    -p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Build failed!
    echo.
    pause
    exit /b 1
)

echo [4/4] Verifying output...

set "OUTPUT_PATH=bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe"

if exist "%OUTPUT_PATH%" (
    echo.
    echo ========================================
    echo  BUILD SUCCESSFUL!
    echo ========================================
    echo.
    echo Output file: %OUTPUT_PATH%
    echo.

    REM Get file size
    for %%A in ("%OUTPUT_PATH%") do (
        set "size=%%~zA"
        set /a "sizeMB=!size! / 1048576"
        echo File size: !sizeMB! MB
    )

    echo.
    echo ----------------------------------------
    echo  USAGE:
    echo ----------------------------------------
    echo   GUI Mode:  Double-click RXReport2HTML.exe
    echo   CLI Mode:  RXReport2HTML.exe "path\to\file.rxlog"
    echo.
    echo ----------------------------------------
    echo  EXAMPLE:
    echo ----------------------------------------
    echo   RXReport2HTML.exe "C:\Tests\TestSuite.rxlog"
    echo.

    REM Ask if user wants to open the output folder
    echo.
    choice /C YN /M "Open output folder"
    if !ERRORLEVEL! EQU 1 (
        explorer "bin\Release\net10.0-windows\win-x64\publish"
    )
) else (
    echo.
    echo [ERROR] Output file not found!
    echo Expected: %OUTPUT_PATH%
    pause
    exit /b 1
)

echo.
echo Build completed successfully!
echo.
pause
