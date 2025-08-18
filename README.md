# 📝 Core_Logic.Application.Services.Logger

A lightweight, extensible logging service for .NET applications. Designed to integrate seamlessly with the built-in dependency injection system.

## 🚀 Features

- Simple logging interface (`ILogger`)
- Customizable log output (e.g., console, file, database)
- Supports dependency injection
- Easily extendable for structured logging

## 📦 Installation

Add the `Logger` class and `ILogger` interface to your project under:

```
Core_Logic.Application.Services
```

Then register it in your `Startup.cs` or `Program.cs`:

```csharp
services.AddSingleton<ILogger, Logger>();
```

If `Logger` has constructor dependencies, make sure those are registered too:

```csharp
services.AddSingleton<IConfigLoader, ConfigLoader>();
services.AddSingleton<ILogger, Logger>();
```

## 🛠️ Usage

Inject `ILogger` into any service or controller:

```csharp
public class MyService
{
    private readonly ILogger _logger;

    public MyService(ILogger logger)
    {
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.Log("Work started.");
    }
}
```

## 🧪 Testing

You can mock `ILogger` for unit tests:

```csharp
var mockLogger = new Mock<ILogger>();
mockLogger.Setup(l => l.Log(It.IsAny<string>()));
```

## 📁 Project Structure

```
Core_Logic.Application.Services/
│
├── ILogger.cs
└── Logger.cs
```

## 📌 Notes

- Ensure all constructor dependencies are registered in DI.
- If you see an error like  
  `"A suitable constructor for type 'Logger' could not be located..."`  
  it means DI can't resolve one of the parameters — check your registrations.

## 📄 License

MIT License. Feel free to use and modify.

```