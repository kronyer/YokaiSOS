using System.Reflection;

namespace Reflection;

public class CliApp
{
    private readonly object _handler;

    public CliApp(object handler)
    {
        _handler = handler; 
    }

    private List<object> _allHandlers;

    public void Run(string[] args)
    {
        LoadHandlers();
        
        if (args.Length == 0)
        {
            PrintHelp(); 
            return;
        }

        if (args[0] == "help" || args[0] == "-h")
        {
            if (args.Length == 1)
            {
                PrintHelp(); 
            }
            else
            {
                PrintCommandHelp(args[1]); // específico
            }
            return;
        }
        var commandName = args[0];
        
        MethodInfo method = null;
        object target = null;

        foreach (var handler in _allHandlers)
        {
            method = handler.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m => m.GetCustomAttribute<CommandAttribute>()?.Name == commandName);

            if (method != null)
            {
                target = handler;
                break;
            }
        }

        if (method == null)
        {
            Console.WriteLine($"Command '{commandName}' not found.");
            return;
        }
        
        var parameters = method.GetParameters();
        var parsedArgs = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            if (i + 1 >= args.Length)
            {
                if (parameters[i].HasDefaultValue)
                {
                    parsedArgs[i] = parameters[i].DefaultValue!;
                    continue;
                }

                Console.WriteLine($"Missing argument for: {parameters[i].Name}");
                return;
            }

            try
            {
                parsedArgs[i] = Convert.ChangeType(args[i+1], parameters[i].ParameterType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing argument '{args[i+1]}': {ex.Message}");
                return;
            }
        }
        method.Invoke(target, parsedArgs);
    }

    private void PrintHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("___________________________________________");
        
        foreach (var handler in _allHandlers)
        {
            var methods = handler.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<CommandAttribute>();
                var parameters = method.GetParameters();

                string paramList = string.Join(" ", parameters.Select(p => $"<{p.ParameterType.Name} {p.Name}>"));
                string desc = string.IsNullOrWhiteSpace(attr.Description) ? "" : $" → {attr.Description}";

                Console.WriteLine($"{attr.Name} {paramList}{desc}");

                foreach (var param in parameters)
                {
                    var paramDesc = param.GetCustomAttribute<ParamDescriptionAttribute>();
                    if (paramDesc != null)
                        Console.WriteLine($"    {param.Name}: {paramDesc.Description}");
                }
            }
        }
    }
    
    private void PrintCommandHelp(string name)
    {
        foreach (var handler in _allHandlers)
        {
            var method = handler.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m => m.GetCustomAttribute<CommandAttribute>()?.Name == name);

            if (method != null)
            {
                var attr = method.GetCustomAttribute<CommandAttribute>();
                var parameters = method.GetParameters();

                string paramList = string.Join(" ", parameters.Select(p => $"<{p.ParameterType.Name} {p.Name}>"));
                string desc = string.IsNullOrWhiteSpace(attr.Description) ? "" : $" → {attr.Description}";

                Console.WriteLine($"{attr.Name} {paramList}{desc}");

                foreach (var param in parameters)
                {
                    var paramDesc = param.GetCustomAttribute<ParamDescriptionAttribute>();
                    if (paramDesc != null)
                        Console.WriteLine($"    {param.Name}: {paramDesc.Description}");
                }

                return;
            }
        }

        Console.WriteLine($"Command '{name}' not found.");
    }

    
    private void LoadHandlers()
    {
        _allHandlers = new List<object> { _handler };

        var pluginFolder = Path.Combine(AppContext.BaseDirectory, "plugins");
        if (!Directory.Exists(pluginFolder)) return;

        foreach (var dll in Directory.GetFiles(pluginFolder, "*.dll"))
        {
            var asm = Assembly.LoadFrom(dll);

            var types = asm.GetTypes().Where(t =>
                typeof(IPluginCommandHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                _allHandlers.Add(instance);
            }
        }
    }
    
    public string GetPrompt()
    {
        if (_handler is CommandHandler handlerWithVfs)
        {
            return $"{handlerWithVfs.GetPrompt()} $";
        }
        return "$";
    }

}