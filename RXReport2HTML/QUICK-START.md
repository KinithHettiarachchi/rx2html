# RXReport2HTML - Quick Start Guide

## Download and Use

1. **Get the executable**: Download `RXReport2HTML.exe`
2. **No installation needed**: It's a self-contained executable

## Two Ways to Use

### 🖱️ GUI Mode (Interactive)
**Just double-click** `RXReport2HTML.exe`
- Drag & drop your `.rxlog` file
- Click "Generate HTML Report"
- Report opens automatically

### ⌨️ CLI Mode (Silent/Automated)
**Run from command line:**
```cmd
RXReport2HTML.exe "C:\path\to\TestSuite.rxlog"
```
- Report saved as `report.html` in the same folder
- No window opens
- Perfect for automation/CI/CD

## Quick Examples

### Single Report
```cmd
RXReport2HTML.exe "C:\Tests\TestSuite_20260504_180555.rxlog"
```

### Batch Processing (PowerShell)
```powershell
# Process all .rxlog files in a folder
Get-ChildItem "C:\Tests" -Filter "*.rxlog" | ForEach-Object {
    & "C:\Tools\RXReport2HTML.exe" $_.FullName
}
```

### Batch Processing (CMD)
```cmd
for /r "C:\Tests" %%f in (*.rxlog) do (
    RXReport2HTML.exe "%%f"
)
```

### Jenkins/CI Integration
```groovy
// In Jenkins pipeline
stage('Generate Reports') {
    steps {
        bat 'RXReport2HTML.exe "%WORKSPACE%\\TestResults\\TestSuite.rxlog"'
        publishHTML([
            reportDir: 'TestResults',
            reportFiles: 'report.html',
            reportName: 'Test Report'
        ])
    }
}
```

## Generated Report Features

✅ **Interactive HTML** with embedded styles and scripts  
✅ **Screenshots** - Click to enlarge, double-click to open  
✅ **Filter** - Show only failed tests  
✅ **Summary** - Test counts, duration, timestamps  
✅ **Single file** - Works offline, easy to share

## Requirements

- Windows 10/11 (64-bit)
- Your `.rxlog` file and companion `.rxlog.data` file

## Troubleshooting

**Error: Data file not found**
- Make sure the `.rxlog.data` file is in the same folder

**Screenshots missing**
- Check that the `images_*` folder exists with screenshots

**Command not recognized**
- Use full path: `C:\Tools\RXReport2HTML.exe "file.rxlog"`
- Or add to PATH environment variable

## File Size

~60-80 MB (self-contained, includes .NET runtime)

## Support

[Add support contact/link here]
