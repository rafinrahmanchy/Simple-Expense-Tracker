# Expense Tracker Visualization Update

## Changes Made

### 1. Added LiveCharts2 Library
- **Package**: LiveChartsCore.SkiaSharpView.WPF (Version 2.0.0-rc2)
- **Purpose**: Provides professional charting capabilities for WPF applications
- **Location**: Added to ExpenseTracker.csproj

### 2. Updated MainViewModel.cs
Added two new visualization features:

#### Pie Chart - Expense Breakdown by Category
- Shows the top 10 expense categories by total amount
- Color-coded slices with percentages
- Data labels showing amounts in currency format
- Legend showing category names and totals

#### Line Chart - Monthly Spending Trends
- Displays spending patterns over time
- Shows total expenses per month
- Visual trend analysis with connected data points
- Helps identify spending patterns and anomalies

### 3. Updated MainWindow.xaml
- Increased window size to 1200x800 to accommodate charts
- Reorganized layout with two rows:
  - **Top Row**: Expense list and input form (existing functionality)
  - **Bottom Row**: Two charts side-by-side
    - Left: Pie Chart (Expense Breakdown)
    - Right: Line Chart (Monthly Trends)

## Features

### Pie Chart Features:
- Automatically groups expenses by description
- Shows top 10 categories
- Real-time updates when expenses are added/modified/deleted
- Currency formatting for amounts
- Color-coded for easy identification

### Line Chart Features:
- Monthly aggregation of expenses
- Chronological ordering
- Visual trend line with markers
- Helps identify spending patterns over time

## How It Works

1. **Automatic Updates**: Charts automatically refresh when:
   - New expenses are added
   - Existing expenses are modified
   - Expenses are deleted
   - Data is loaded

2. **Data Processing**:
   - Pie Chart: Groups by expense description, sums amounts, shows top 10
   - Line Chart: Groups by month, sums monthly totals, orders chronologically

3. **Visual Design**:
   - Clean, modern appearance
   - Consistent with existing application design
   - Professional chart styling
   - Responsive layout

## Building the Application

To build and run the updated application:

```bash
dotnet restore
dotnet build
dotnet run
```

Or open in Visual Studio and build/run from there.

## Dependencies

- .NET 6.0 or higher
- WPF
- LiveChartsCore.SkiaSharpView.WPF 2.0.0-rc2

## Notes

- The charts will be empty if no expenses exist
- Add some test data to see the visualizations in action
- The pie chart shows top 10 categories to avoid overcrowding
- Colors cycle through a predefined palette for consistency
