@echo off
REM Installation and Build Helper for Expense Tracker (.NET 10)
REM This script provides adaptive environment installation support

setlocal enabledelayedexpansion

echo.
echo ========================================
echo  Expense Tracker - Installation Helper
echo  .NET 10 Edition
echo ========================================
echo.

REM Check if .NET SDK is installed
echo Checking .NET SDK installation...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET 10.0 SDK from https://dotnet.microsoft.com/download/dotnet/10.0
    echo.
    pause
    exit /b 1
)

dotnet --version
echo.

REM Check minimum .NET version
for /f "tokens=1" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo Detected .NET SDK Version: !DOTNET_VERSION!
echo.

REM Clean previous build artifacts
echo Cleaning previous build artifacts...
dotnet clean ExpenseTracker.csproj >nul 2>&1
echo Done.
echo.

REM Restore NuGet packages
echo Restoring NuGet packages...
dotnet restore ExpenseTracker.csproj
if errorlevel 1 (
    echo ERROR: Failed to restore NuGet packages
    pause
    exit /b 1
)
echo Done.
echo.

REM Build the project
echo Building Expense Tracker for .NET 10...
dotnet build ExpenseTracker.csproj -c Release
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)
echo.
echo Build completed successfully!
echo.

REM Run the application
echo Starting Expense Tracker...
dotnet run --project ExpenseTracker.csproj --configuration Release

endlocal
pause
