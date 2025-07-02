namespace Reflection;

public class CommandHandler
{
    private readonly VirtualFileSystem _vfs;
    
    public CommandHandler(VirtualFileSystem vfs)
    {
        _vfs = vfs;
    }
    
    [Command("touch", Description = "Cria um arquivo vazio")]
    public void Touch(
        [Option("name")] [ParamDescription("Nome do arquivo")] string name,
        [Option("content")] [ParamDescription("Conteúdo opcional")] string content = "")
    {
        try
        {
            if (_vfs.List().Any(n => n.Name == name))
            {
                Console.WriteLine($"Arquivo '{name}' já existe.");
                return;
            }

            _vfs.CreateFile(name, content);
            Console.WriteLine($"Arquivo '{name}' criado.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
    
    [Command("ls", Description = "Lista arquivos e diretórios")]
    public void Ls()
    {
        var itens = _vfs.List();

        foreach (var item in itens)
        {
            var tipo = item is DirectoryNode ? "[DIR]" : "[FILE]";
            Console.WriteLine($"{tipo} {item.Name}");
        }
    }
    
    [Command("cd", Description = "Altera o diretório atual")]
    public void Cd([ParamDescription("Nome do diretório ou '/' para raiz")] string nome)
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


    
    [Command("mkdir", Description = "Cria um diretório")]
    public void Mkdir([ParamDescription("Nome do diretório")] string name)
    {
        try
        {
            _vfs.MakeDirectory(name);
            Console.WriteLine($"Diretório '{name}' criado.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
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

}