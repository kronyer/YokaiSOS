using YokaiSOS.Core;

namespace YokaiSOS.Plugin.Hello;

public class HelloPlugin : IPluginCommandHandler
{
    [Command("hello", Description = "Says hello to a person")]
    public void Hello([ParamDescription("Name of the person to say hello to")] string name)
    {
        Console.WriteLine($"[plugin] Hello, {name}!");
    }
}