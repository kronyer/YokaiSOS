namespace YokaiSOS.Core;

public class DirectoryNode : FsNode
{
    public List<FsNode> Children { get; } = new List<FsNode>();
    
    public FsNode? GetChild(string name) =>
        Children.FirstOrDefault(child => child.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void AddChild(FsNode node)
    {
        node.Parent = this;
        Children.Add(node);
    }
    public void RemoveChild(FsNode node)
    {
        Children.Remove(node);
    }
}