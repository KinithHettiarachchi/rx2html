# Build Self-Contained Single EXE for RXReport2HTML
# This script builds a self-contained single executable file

Write-Host "Building RXReport2HTML..." -ForegroundColor Cyan

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean -c Release

# Build and publish as single file
Write-Host "Publishing self-contained single executable..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild successful!" -ForegroundColor Green
    Write-Host "`nOutput location:" -ForegroundColor Cyan
    Write-Host "  bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe" -ForegroundColor White

    # Get file size
    $exePath = "bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe"
    if (Test-Path $exePath) {
        $fileSize = (Get-Item $exePath).Length / 1MB
        Write-Host "`nFile size: $([math]::Round($fileSize, 2)) MB" -ForegroundColor White
    }

    Write-Host "`nUsage:" -ForegroundColor Cyan
    Write-Host "  GUI Mode: Double-click RXReport2HTML.exe" -ForegroundColor White
    Write-Host "  CLI Mode: RXReport2HTML.exe ""path\to\file.rxlog""" -ForegroundColor White
} else {
    Write-Host "`nBuild failed!" -ForegroundColor Red
    exit 1
}
