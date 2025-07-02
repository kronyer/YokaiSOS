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
        ShowBootArt();
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

        Console.WriteLine("I'm watching you...");
    }

    
    
    private static string[] ParseInput(string input)
    {
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
    
    private void ShowBootArt()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("▁▂▃▄▅▆▇█ YOKAI SOS █▇▆▅▄▃▂▁");
        Console.WriteLine(" Simulated Operational System");
        Console.WriteLine(" ────────────────");
        Console.WriteLine("  無 • 靈 • 情 • 操 • 幻 • 魂");
        Console.WriteLine(" ────────────────");
        Console.WriteLine(" 'Everything is virtual.");
        Console.WriteLine("  Nothing touches the host.");
        Console.WriteLine("  But everything responds.'");
        Console.WriteLine();
        Console.WriteLine(" Type `help` to begin.");
        Console.WriteLine(" Type `summon` to awaken forgotten things.");
        Console.WriteLine();
    }

}
