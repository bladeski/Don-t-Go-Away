# 🟢 Don't Go Away

A lightweight Windows utility that prevents Microsoft Teams from entering the "Away" status by simulating user activity. Ideal for remote workers, on-call engineers, or anyone who wants to stay green without constant interaction.

---

## 💡 Why Use This?

Microsoft Teams automatically sets your status to "Away" after a period of inactivity. This app keeps your presence active by mimicking subtle user behavior — no hacks, no mouse jigglers, just smart background activity.

---

## 🚀 Features

- 🟢 Keeps Teams status set to "Available"
- 🕒 Runs silently in the background
- ⚙️ Configurable activity intervals
- 📄 Custom logging system for diagnostics
- 🖥️ Minimal resource usage
- 📦 Packaged as a Windows `.msix` installer

---

## 🛠️ Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- [Windows Presentation Foundation (WPF)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- Custom-built logging framework
- MSIX Packaging for clean Windows distribution

---

## 📦 Installation

Download the latest `.msix` installer from the [Releases](https://github.com/your-username/dont-go-away/releases) page and double-click to install.

Once installed, the app runs in the background and keeps your Teams status active.

---

## 🧪 Development Setup

```bash
git clone https://github.com/your-username/dont-go-away.git
cd dont-go-away
dotnet restore
dotnet build


To run the app:
dotnet run --project "Don't Go Away/Don't Go Away.csproj"

You can configure verbosity and log output in Logging/Logger.cs.

📃 License
MIT License. See LICENSE for details.

🙌 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you'd like to change.
