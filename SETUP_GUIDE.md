# Expense Tracker - .NET 10 Installation & Setup Guide

## System Requirements

### Minimum Requirements
- **Operating System:** Windows 7 SP1 or later (Windows 10/11 recommended)
- **.NET Runtime:** .NET 10.0 Desktop Runtime or SDK
- **RAM:** 512 MB minimum
- **Disk Space:** 500 MB for installation

### Recommended Requirements
- **Operating System:** Windows 10/11 (latest versions)
- **.NET Runtime:** .NET 10.0 latest build
- **RAM:** 2 GB or more
- **Disk Space:** 1 GB free space
- **Display:** 1920x1080 or higher resolution

## Pre-Installation Checklist

Before installing Expense Tracker, ensure:

1. ✅ Windows is fully updated
2. ✅ Administrator access is available
3. ✅ At least 500 MB of free disk space
4. ✅ Internet connection (for downloading .NET SDK if needed)

## Installation Methods

### Method 1: Automated Installation (Recommended)

This is the simplest method for most users.

#### Windows Users:

1. **Download and Extract**
   - Clone or download the repository
   - Extract the files to a preferred location (e.g., `C:\Users\YourName\Desktop\ExpenseTracker`)

2. **Run the Installation Script**
   ```batch
   cd Simple-Expense-Tracker
   build-and-run.bat
   ```

   The script automatically:
   - Checks .NET 10 SDK installation
   - Restores NuGet packages
   - Builds the application
   - Launches the app

3. **First Launch**
   - The application will create necessary files automatically
   - Your data is stored in `expenses.json`

### Method 2: Manual Installation

#### Step 1: Install .NET 10 SDK

**Option A: Using Windows Installer (Easiest)**
1. Visit https://dotnet.microsoft.com/download/dotnet/10.0
2. Download `.NET 10.0 SDK` (Windows installer)
3. Run the installer and follow the on-screen instructions
4. Close and reopen Command Prompt for changes to take effect

**Option B: Using Windows Package Manager (Winget)**
```batch
winget install Microsoft.DotNet.SDK.10
```

**Option C: Using Chocolatey**
```batch
choco install dotnet-sdk-10.0
```

#### Step 2: Verify Installation

Open Command Prompt or PowerShell and run:
```bash
dotnet --version
```

You should see version 10.0.x or higher.

#### Step 3: Clone the Repository

```bash
git clone https://github.com/rafinrahmanchy/Simple-Expense-Tracker.git
cd Simple-Expense-Tracker
```

#### Step 4: Build the Project

```bash
dotnet restore
dotnet build -c Release
```

#### Step 5: Run the Application

```bash
dotnet run --project ExpenseTracker.csproj
```

### Method 3: Publish as Standalone Executable

Create a self-contained executable that doesn't require .NET to be installed separately:

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

The executable will be in `bin\Release\net10.0-windows\win-x64\publish\ExpenseTracker.exe`

#### Publish Options

**For Latest .NET (Dependency on .NET Runtime)**
```bash
dotnet publish -c Release -o ./publish
```

**For Self-Contained (No Dependencies)**
```bash
dotnet publish -c Release -r win-x64 --self-contained -o ./publish
```

**For Smaller File Size**
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishTrimmed=true
```

## Adaptive Environment Configuration

Expense Tracker .NET 10 includes adaptive environment support for different installation scenarios.

### Configuration File: appsettings.json

Located in the application root directory, this file allows customization:

```json
{
  "DataStorage": {
    "FileName": "expenses.json",
    "UseApplicationDirectory": true,
    "CreateBackups": true,
    "Adaptive": {
      "EnableAutoPathDetection": true,
      "AllowCustomPaths": false
    }
  }
}
```

### Adaptive Path Detection

When `EnableAutoPathDetection` is enabled:
- **User Directory:** Data is stored in `%APPDATA%\Expense Tracker\`
- **Application Directory:** Data is stored alongside the executable
- **Network Paths:** Supported for shared installations
- **Custom Paths:** Can be enabled in configuration for enterprise deployments

### Storage Paths by Environment

| Environment | Default Path |
|-------------|--------------|
| User Directory | `C:\Users\[Username]\AppData\Roaming\Expense Tracker\` |
| Application Directory | `[InstallPath]\expenses.json` |
| Portable USB | Root of USB drive or custom configured path |
| Network Share | UNC path (requires proper permissions) |

## Troubleshooting

### Problem: ".NET SDK not found"

**Solution:**
```bash
dotnet --info
```

If .NET 10 is not listed, install it from https://dotnet.microsoft.com/download/dotnet/10.0

### Problem: "Cannot build WPF application"

**Solution:**
Ensure you're using the correct Windows SDK:
```bash
dotnet --list-sdks
dotnet --list-runtimes
```

### Problem: "Application won't start"

**Try:**
1. Verify .NET runtime is installed: `dotnet --version`
2. Check Windows Event Viewer for error details
3. Run from Command Prompt for detailed error messages
4. Delete old `expenses.json` and try again

### Problem: "Data not persisting"

**Check:**
1. Verify write permissions in the installation directory
2. Ensure disk space is available
3. Check that `expenses.json` is not open in another program
4. Review the log file if enabled in `appsettings.json`

### Problem: "Build fails with package errors"

**Solution:**
```bash
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

## Advanced Configuration

### Enable Logging

Edit `appsettings.json`:
```json
{
  "Logging": {
    "EnableFileLogging": true,
    "LogFilePath": "logs/expensetracker.log"
  }
}
```

Logs will be created in the `logs` subdirectory.

### Custom Data Storage Location

**For portable installation:**
```json
{
  "DataStorage": {
    "FileName": "expenses.json",
    "UseApplicationDirectory": true,
    "Adaptive": {
      "EnableAutoPathDetection": false,
      "AllowCustomPaths": true
    }
  }
}
```

### Network/Shared Installation

For shared network installations:

1. Deploy to network share: `\\server\expense-tracker\`
2. Ensure all users have read/write permissions
3. Configure shared `appsettings.json`
4. Each user can run the application with their own data

## Updating from .NET 6.0

If upgrading from the .NET 6.0 version:

1. **Backup your data:**
   ```bash
   copy expenses.json expenses.json.backup
   ```

2. **Close the old application**

3. **Install .NET 10 SDK** (if not already installed)

4. **Clone or update the repository:**
   ```bash
   git pull origin main
   ```

5. **Build and run:**
   ```bash
   dotnet build
   dotnet run
   ```

Your existing `expenses.json` file is fully compatible and will be automatically loaded.

## Performance Optimization

### For Better Performance

1. **Use Release Build:**
   ```bash
   dotnet run -c Release
   ```

2. **Publish as Self-Contained:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained
   ```

3. **Trim Unused Code (Reduces Size):**
   ```bash
   dotnet publish -c Release -p:PublishTrimmed=true
   ```

### Expected Performance

- **Startup Time:** < 1 second
- **Load 10,000 Expenses:** < 500ms
- **Add/Update Expense:** < 100ms
- **Generate Monthly Summary:** < 200ms

## Uninstallation

### Remove the Application

1. Delete the application folder
2. (Optional) Delete data folder: `%APPDATA%\Expense Tracker\`

### Keep Your Data

The application data is stored in `expenses.json`. You can:
- Backup this file before uninstalling
- Import it into a newer version later

## Support & Feedback

- **GitHub Issues:** https://github.com/rafinrahmanchy/Simple-Expense-Tracker/issues
- **Email:** rafinrahman24@gmail.com
- **Discussion:** https://github.com/rafinrahmanchy/Simple-Expense-Tracker/discussions

## Version Information

- **Current Version:** 2.0.0 (.NET 10)
- **.NET Framework:** .NET 10.0-windows
- **C# Language:** C# 13
- **Build Date:** 2026
- **License:** MIT

---

**Last Updated:** March 2026
**Status:** ✅ Production Ready
