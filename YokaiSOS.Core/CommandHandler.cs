namespace YokaiSOS.Core;

public class CommandHandler
{
    private readonly VirtualFileSystem _vfs;
    
    public CommandHandler(VirtualFileSystem vfs)
    {
        _vfs = vfs;
    }
    
    [Command("touch", Description = "Creates an empty file")]
    public void Touch(
        [Option("name")] [ParamDescription("Name of the file")] string name,
        [Option("content")] [ParamDescription("Optional content")] string content = "")
    {
        try
        {
            if (_vfs.List().Any(n => n.Name == name))
            {
                Console.WriteLine($"Arquivo '{name}' já existe.");
                return;
            }

            _vfs.CreateFile(name, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating the '{name}' file: {ex.Message}");
        }
    }
    
    [Command("ls", Description = "Lists the files and directories in the current directory")]
    public void Ls()
    {
        var itens = _vfs.List();

        foreach (var item in itens)
        {
            var tipo = item is DirectoryNode ? "[DIR]" : "[FILE]";
            Console.WriteLine($"{tipo} {item.Name}");
        }
    }
    
    [Command("cd", Description = "Change the current directory")]
    public void Cd([ParamDescription("Name of directory or '/' to root")] string nome)
    {
        try
        {
            _vfs.ChangeDirectory(nome);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }


    
    [Command("mkdir", Description = "Creates a directory")]
    public void Mkdir([ParamDescription("Nome do diretório")] string name)
    {
        try
        {
            _vfs.MakeDirectory(name);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating '{name}': {ex.Message}");
        }
    }
    
    [Command("greet", Description = "Greets a user")]
    public void Greet([ParamDescription("Name of the person that will be greeted")] string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }
    
    [Command("add", Description = "Adds a pair of integers")]
    public void Add([ParamDescription("First number of the operation")] int a,
        [ParamDescription("Second number of the operation")] int b)
    {
        Console.WriteLine($"The sum of {a} and {b} is {a + b}.");
    }
    
    public string GetPrompt()
    {
        return _vfs.PrintWorkingDirectory();
    }
    
    [Command("cat", Description = "Displays the content of a file")]
    public void Cat(string name)
    {
        var file = _vfs.ResolveFile(name);
        if (file == null)
        {
            Console.WriteLine($"File '{name}' not found.");
            return;
        }

        Console.WriteLine(file.Content);
    }
    
    [Command("rm", Description = "Remove a file or empty directory")]
    public void Rm(string name, [Option("r")] bool recursive = false, [Option("f")] bool force = false)
    {
        try
        {
            _vfs.Remove(name, recursive, force);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on removing '{name}': {ex.Message}");
        }
    }



}