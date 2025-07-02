namespace Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; set; }

    public CommandAttribute(string name)
    {
        Name = name;
    }
    
    public CommandAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}