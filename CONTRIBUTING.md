# Contributing to Expense Tracker

Thank you for your interest in contributing to the Expense Tracker project! This document provides guidelines and instructions for contributing.

## Code of Conduct

- Be respectful and inclusive
- Welcome diverse perspectives and ideas
- Focus on constructive feedback
- Report unacceptable behavior to the maintainers

## How to Contribute

### Reporting Bugs

Before creating a bug report, please check the issue list to ensure the bug hasn't already been reported.

When reporting a bug, include:
- **Clear description** of the issue
- **Steps to reproduce** the problem
- **Expected behavior** vs actual behavior
- **Screenshots** if applicable
- **System information:** OS, .NET version, etc.
- **Error messages** or logs

### Suggesting Enhancements

Enhancement suggestions are welcome! Please provide:
- **Clear description** of the enhancement
- **Motivation** for the feature
- **Examples** of how it would be used
- **Implementation considerations** if applicable

### Pull Requests

1. **Fork the Repository**
   ```bash
   git clone https://github.com/yourusername/ExpenseTracker.git
   cd ExpenseTracker
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Follow the code style guidelines (see below)
   - Write clear, descriptive commit messages
   - Add XML documentation for new public methods
   - Update README.md if adding features

4. **Test Your Changes**
   ```bash
   dotnet build
   dotnet test  # if applicable
   ```

5. **Commit Your Changes**
   ```bash
   git commit -m "Description of your changes"
   ```

6. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **Create a Pull Request**
   - Provide a clear title and description
   - Reference any related issues
   - Ensure all tests pass

## Code Style Guidelines

### C# Coding Standards

We follow [Microsoft's C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

- **Naming:**
  - `PascalCase` for class names, properties, and public methods
  - `camelCase` for local variables and private fields
  - `_camelCase` for private fields
  - `UPPER_CASE` for constants (if not using PascalCase for properties)

- **Formatting:**
  - 4 spaces for indentation (no tabs)
  - Opening braces on the same line
  - Use `var` for obvious types, explicit types otherwise

- **Documentation:**
  - Add XML documentation comments for public classes and methods
  - Use `///` for documentation comments
  - Include parameter descriptions and return value information

Example:
```csharp
/// <summary>
/// Saves a collection of expenses to the JSON file.
/// </summary>
/// <param name="expenses">The collection of expenses to save.</param>
/// <exception cref="ArgumentNullException">Thrown when expenses is null.</exception>
public void SaveData(IEnumerable<Expense> expenses)
{
    if (expenses == null)
        throw new ArgumentNullException(nameof(expenses));
    
    // Implementation
}
```

### XAML Guidelines

- Use meaningful names for controls
- Organize properties logically
- Keep formatting consistent
- Use data binding instead of code-behind when possible

### Best Practices

- Use `using` statements for resource management
- Implement `IDisposable` for unmanaged resources
- Use async/await for long-running operations
- Implement proper null checks
- Follow DRY (Don't Repeat Yourself) principle
- Keep methods focused and small
- Use meaningful variable and method names

## Project Structure

```
ExpenseTracker/
├── Models/           # Data models
├── Services/         # Business logic & file operations
├── ViewModels/       # MVVM logic
├── Views/            # XAML files (future)
├── Tests/            # Unit tests (future)
└── docs/             # Documentation (future)
```

## Development Workflow

1. **Create Feature Branch** from `main`
2. **Develop & Test** your changes
3. **Commit with Clear Messages**
4. **Push to Fork** and Create Pull Request
5. **Address Review Comments** if any
6. **Merge** when approved

## Commit Message Guidelines

- Use clear, descriptive messages
- Start with a verb: "Add", "Fix", "Update", "Refactor", "Remove"
- Keep the first line under 50 characters
- Add detailed description if needed

Examples:
```
Add monthly expense summary feature
Fix null reference exception in SelectedExpense setter
Update documentation for FileService class
Refactor MainViewModel command initialization
Remove deprecated LoadDataAsync method
```

## Testing

- Write unit tests for new features
- Ensure all existing tests pass
- Test edge cases and error scenarios
- Use meaningful test names

## Documentation

- Update README.md for new features
- Add inline comments for complex logic
- Include XML documentation for public APIs
- Keep documentation up-to-date

## License

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

## Questions?

- Check existing issues and discussions
- Create a new discussion for questions
- Open an issue if you need clarification

## Recognition

Contributors will be recognized in the project README and release notes.

---

Thank you for making Expense Tracker better! 🎉
