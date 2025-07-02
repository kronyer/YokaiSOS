using System.Text;

namespace Reflection;

using System;

public class Terminal
{
    private readonly CliApp _cli;

    public Terminal(CliApp cli)
    {
        _cli = cli;
    }

    public void Start()
    {
        Console.WriteLine("🔹 Sistema interativo iniciado. Digite 'exit' para sair.");

        while (true)
        {
            Console.Write($"{_cli.GetPrompt()} ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Trim().ToLower() == "exit")
                break;

            var args = ParseInput(input);
            try
            {
                _cli.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[erro]: {ex.Message}");
            }
        }

        Console.WriteLine("Sistema encerrado.");
    }

    private static string[] ParseInput(string input)
    {
        // divide por espaço mas respeita aspas
        var tokens = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        foreach (var c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (char.IsWhiteSpace(c) && !inQuotes)
            {
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
            tokens.Add(current.ToString());

        return tokens.ToArray();
    }
}
