# RXReport2HTML

Convert Ranorex test reports (.rxlog) to beautiful standalone HTML reports with embedded screenshots and interactive filtering.

🔗 **Repository**: https://github.com/KinithHettiarachchi/rx2html

## ✨ Features

- 🖥️ **Dual Mode**: GUI (drag & drop) + CLI (automation)
- 📦 **Self-Contained**: Single EXE, no dependencies
- 🎨 **Professional Reports**: Clean light theme with embedded styles
- 📸 **Screenshot Support**: Embedded with overlay viewer
- 🔍 **Smart Filtering**: Toggle to show only failed tests
- ⚡ **Fast & Efficient**: Generates reports in seconds

## 🚀 Quick Start

### Download
Get the latest release from [Releases](https://github.com/KinithHettiarachchi/rx2html/releases)

### GUI Mode
Double-click `RXReport2HTML.exe` → Drag & drop `.rxlog` file → Generate!

### CLI Mode
```cmd
RXReport2HTML.exe "C:\path\to\TestSuite.rxlog"
```

📖 **Full documentation**: See [RXReport2HTML/README.md](RXReport2HTML/README.md)

## 🛠️ Building from Source

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022/2026 or VS Code

### Build Single EXE
```cmd
build.bat
```

Output: `RXReport2HTML\bin\Release\net10.0-windows\win-x64\publish\RXReport2HTML.exe`

## 📁 Project Structure

```
rx2html/
├── build.bat                    # Build script
├── .gitignore                   # Git ignore rules
└── RXReport2HTML/               # Main project
    ├── Form1.cs                 # Core logic
    ├── Program.cs               # Entry point
    └── RXReport2HTML.csproj     # Project file
```

See [PROJECT-STRUCTURE.md](PROJECT-STRUCTURE.md) for details.

## 📚 Documentation

- **[QUICK-START.md](RXReport2HTML/QUICK-START.md)** - Quick reference guide
- **[README.md](RXReport2HTML/README.md)** - Detailed usage instructions
- **[PROJECT-STRUCTURE.md](PROJECT-STRUCTURE.md)** - Project organization

## 🤝 Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## 📄 License

[Add your license here]

## 🐛 Issues & Support

Report issues: https://github.com/KinithHettiarachchi/rx2html/issues

---

Made with ❤️ for Ranorex test automation
