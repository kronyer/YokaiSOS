namespace YokaiSOS.Core;
[AttributeUsage(AttributeTargets.Parameter)]
public class OptionAttribute : Attribute
{
    public string Name { get; set; }

    public OptionAttribute(string name)
    {
        Name = name;
    }
}