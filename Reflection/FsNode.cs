namespace Reflection;

public abstract class FsNode
{
    public string Name { get; set; }
    public DirectoryNode Parent { get; set; }

    public string GetPath()
    {
        return Parent == null ? Name : $"{Parent.GetPath().TrimEnd('/')}/{Name}";
    }
}