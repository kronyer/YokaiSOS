namespace YokaiSOS.Core;

public class ParamDescriptionAttribute : Attribute
{
    public string Description { get; set; }

    public ParamDescriptionAttribute(string description)
    {
        Description = description;
    }
}