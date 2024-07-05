using System.Text;

namespace OllamaCommitGen.Cli.Utils;

public static class ConsoleWrapper
{
    public static string WriteEditableLine(string text)
    {
        var initialPosition = Console.GetCursorPosition().Left;
        Console.Write(text);
        var sb = new StringBuilder(text);
        var cursorPosition = text.Length + initialPosition;
        ConsoleKeyInfo keyInfo;

        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow when cursorPosition > initialPosition:
                    Console.SetCursorPosition(--cursorPosition, Console.CursorTop);
                    break;
                case ConsoleKey.RightArrow when cursorPosition < sb.Length + initialPosition:
                    Console.SetCursorPosition(++cursorPosition, Console.CursorTop);
                    break;
                case ConsoleKey.Home:
                    cursorPosition = initialPosition;
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    break;
                case ConsoleKey.End:
                    cursorPosition = sb.Length + initialPosition;
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    break;
                case ConsoleKey.Backspace when cursorPosition > initialPosition:
                    sb = sb.Remove(cursorPosition - initialPosition - 1, 1);
                    ClearFromTo(initialPosition, initialPosition + sb.Length + 1);
                    Console.Write(sb.ToString());
                    Console.SetCursorPosition(--cursorPosition, Console.CursorTop);
                    break;
                case ConsoleKey.Delete when cursorPosition != sb.Length + initialPosition:
                    sb = sb.Remove(cursorPosition - initialPosition, 1);
                    ClearFromTo(initialPosition, initialPosition + sb.Length + 1);
                    Console.Write(sb.ToString());
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    break;
                default:
                    if (char.IsLetterOrDigit(keyInfo.KeyChar) || keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        sb = sb.Insert(cursorPosition - initialPosition, keyInfo.KeyChar);
                        ClearFromTo(initialPosition, initialPosition + sb.Length - 1);
                        Console.Write(sb.ToString());
                        Console.SetCursorPosition(++cursorPosition, Console.CursorTop);
                    }
                    break;
            }

        Console.Write(Environment.NewLine);

        return sb.ToString();
    }

    private static void ClearFromTo(int from, int to)
    {
        Console.SetCursorPosition(to, Console.CursorTop);
        
        while (to != from)
        {
            Console.Write("\b \b");
            --to;
            Console.SetCursorPosition(to, Console.CursorTop);
        }
    }
}