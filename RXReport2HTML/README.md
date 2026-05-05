# RXReport2HTML - Ranorex Report to HTML Converter

This application converts Ranorex test reports (.rxlog files) into standalone HTML reports with embedded screenshots and filtering capabilities.

## Features

- **Dual Mode Operation**: Works both as a GUI application and command-line tool
- **Self-Contained**: Single executable file with no dependencies
- **Interactive HTML Reports**: Beautiful light-mode reports with filtering
- **Screenshot Support**: Embedded screenshots with overlay viewer
- **Failed Test Filtering**: Toggle to show only failed tests

## Building the Application

### Build as Self-Contained Single EXE

To create a self-contained single executable file:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The output will be in: `bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe`

### Build Options

For different target platforms:
- **Windows x64**: `-r win-x64`
- **Windows x86**: `-r win-x86`
- **Windows ARM64**: `-r win-arm64`

## Usage

### GUI Mode (Double-Click)

Simply double-click `RXReport2HTML.exe` to open the graphical interface:
1. Drag and drop a `.rxlog` file onto the window, or
2. Click "Browse" to select a `.rxlog` file
3. Click "Generate HTML Report"
4. The report will open automatically in your default browser

### Command-Line Mode (Silent)

Generate reports from the command line without opening the GUI:

```bash
RXReport2HTML.exe "C:\path\to\TestSuite.rxlog"
```

The report will be saved as `report.html` in the same directory as the `.rxlog` file.

#### Command-Line Examples

```bash
# Single file
RXReport2HTML.exe "C:\Tests\TestSuite_20260504_180555.rxlog"

# Using relative path
RXReport2HTML.exe ".\reports\TestSuite.rxlog"

# Batch processing (PowerShell)
Get-ChildItem -Path "C:\Tests" -Filter "*.rxlog" -Recurse | ForEach-Object {
    & "C:\Tools\RXReport2HTML.exe" $_.FullName
}
```

## Generated Report Features

### Summary Section
- Test Suite name
- Overall result status
- Total tests, passed, and failed counts
- Duration and timestamps (Started/Ended)
- **Filter checkbox**: Toggle to show only failed tests

### Test Execution Details
- Hierarchical test structure
- Color-coded results (Success/Warning/Failure)
- Duration badges
- Log messages with severity indicators
- Embedded screenshots with:
  - Click to view in overlay
  - Double-click to open in default image viewer

## Requirements

### For Running the Application
- Windows 10/11 (64-bit)
- No additional dependencies (self-contained)

### For Building from Source
- .NET 10 SDK
- Visual Studio 2022/2026 or VS Code

## File Structure

The application expects the following files in the same directory as the `.rxlog` file:
- `<name>.rxlog` - Entry point file
- `<name>.rxlog.data` - XML data file (required)
- `RanorexReport.css` - Stylesheet (optional)
- `images_*\` - Screenshot folder (optional)

## Output

The generated HTML report includes:
- All CSS styles embedded
- All JavaScript embedded
- All screenshots embedded as Base64
- **Single file output** - no external dependencies
- Works offline

## Troubleshooting

### "Data file not found" error
Ensure the `.rxlog.data` file exists in the same directory with the same base name as the `.rxlog` file.

### Screenshots not showing
Check that the `images_*` folder exists and contains the screenshot files referenced in the `.rxlog.data` XML.

### Command-line mode not working
Make sure to enclose file paths in quotes if they contain spaces.

## License

[Add your license information here]
