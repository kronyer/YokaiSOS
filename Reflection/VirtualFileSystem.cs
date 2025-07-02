namespace Reflection;

public class VirtualFileSystem
{
    public DirectoryNode Root { get; }
    public DirectoryNode Current { get; private set; }

    public VirtualFileSystem()
    {
        Root = new DirectoryNode { Name = "/" };
        Current = Root;

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
            throw new Exception($"Directory '{name}' not found.");

        Current = target;
    }

    public void MakeDirectory(string name)
    {
        if (Current.GetChild(name) != null)
            throw new Exception($"'{name}' already exists.");

        var dir = new DirectoryNode { Name = name };
        Current.AddChild(dir);
    }

    public void CreateFile(string name, string content = "")
    {
        if (Current.GetChild(name) != null)
            throw new Exception($"'{name}' already exists.");

        var file = new FileNode { Name = name, Content = content };
        Current.AddChild(file);
    }
    
    public void Remove(string name, bool recursive = false, bool force = false)
    {
        var node = Current.GetChild(name);

        if (node == null)
        {
            if (force)
                return;
            throw new Exception($"'{name}' not found.");
        }

        if (node is DirectoryNode dir)
        {
            if (!recursive && dir.Children.Any())
                throw new Exception($"Directory '{name}' is not empty, use -r to remove recursively.");

            foreach (var child in dir.Children.ToList())
            {
                var prev = Current;
                Current = dir;
                Remove(child.Name, recursive, force);
                Current = prev;
            }
        }

        Current.RemoveChild(node);
    }
    
    public FileNode? ResolveFile(string name)
    {
        return Current.GetChild(name) as FileNode;
    }

    public FsNode? ResolveNode(string name)
    {
        return Current.GetChild(name); 
    }

    public IEnumerable<FsNode> List() => Current.Children;

    public string PrintWorkingDirectory() => Current.GetPath();
}
