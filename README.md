# YokaiSOS

YokaiSOS is a modular, libre software .NET application that leverages C# reflection to provide a flexible, extensible command-line interface. Inspired by a passion for yokai (supernatural creatures from Japanese folklore), the project blends technical exploration of .NET's reflection capabilities with creative, yokai-themed features and plugins.

## Project Idea
YokaiSOS aims to be a playground for both software engineering and folklore enthusiasts. The core application uses C# reflection to dynamically discover, load, and execute commands and plugins, making it easy to extend with new functionality. Plugins can introduce new commands, behaviors, or even virtual yokai entities, encouraging experimentation and creativity.

## Features
- Command-line interface with attribute-based command and option definitions
- Dynamic plugin loading from the `plugins/` directory
- Virtual file system abstraction
- Yokai-themed extensibility: plugins can add new yokai, lore, or supernatural abilities

## Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Building
1. Clone the repository.
2. Build the solution:
   ```sh
   dotnet build YokaiSOS.sln
   ```

### Running
1. Navigate to the output directory:
   ```sh
   cd YokaiSOS.Core/bin/Debug/net9.0
   ```
2. Run the application:
   ```sh
   ./Reflection
   ```

### Plugins
- Place plugin DLLs in the `YokaiSOS.Core/bin/Debug/net9.0/plugins/` directory.
- Example plugin source: `Plugin.Hello/HelloPlugin.cs`

## Project Structure
- `YokaiSOS.Core/` - Core application source code
- `Plugin.Hello/` - Example plugin
- `YokaiSOS.sln` - Solution file

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is libre software. See the LICENSE file for details.
