namespace Reflection;

public class VirtualFileSystem
{
    public DirectoryNode Root { get; }
    public DirectoryNode Current { get; private set; }

    public VirtualFileSystem()
    {
        Root = new DirectoryNode { Name = "/" };
        Current = Root;

        // Estrutura padrão
        var home = new DirectoryNode { Name = "home" };
        var bin = new DirectoryNode { Name = "bin" };
        var etc = new DirectoryNode { Name = "etc" };
        var tmp = new DirectoryNode { Name = "tmp" };
        var user = new DirectoryNode { Name = "pedro" };

        Root.AddChild(home);
        Root.AddChild(bin);
        Root.AddChild(etc);
        Root.AddChild(tmp);
        home.AddChild(user);

        // Setar home do usuário como diretório inicial
        Current = user;
    }

    public void ChangeDirectory(string name)
    {
        if (name == "/")
        {
            Current = Root;
            return;
        }
        
        if (name == "..")
        {
            if (Current.Parent != null)
                Current = Current.Parent;
            return;
        }

        var target = Current.GetChild(name) as DirectoryNode;
        if (target == null)
            throw new Exception($"Diretório '{name}' não encontrado.");

        Current = target;
    }

    public void MakeDirectory(string name)
    {
        if (Current.GetChild(name) != null)
            throw new Exception($"'{name}' já existe.");

        var dir = new DirectoryNode { Name = name };
        Current.AddChild(dir);
    }

    public void CreateFile(string name, string content = "")
    {
        if (Current.GetChild(name) != null)
            throw new Exception($"'{name}' já existe.");

        var file = new FileNode { Name = name, Content = content };
        Current.AddChild(file);
    }

    public IEnumerable<FsNode> List() => Current.Children;

    public string PrintWorkingDirectory() => Current.GetPath();
}
