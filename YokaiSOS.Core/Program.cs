using YokaiSOS.Core;


var vfs = new VirtualFileSystem();
var handler = new CommandHandler(vfs);
var cli = new CliApp(handler);

if (args.Length > 0)
{
    cli.Run(args);
}
else
{
    var terminal = new Terminal(cli);
    terminal.Start();
}