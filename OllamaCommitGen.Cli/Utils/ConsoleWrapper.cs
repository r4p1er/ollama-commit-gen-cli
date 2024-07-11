using System.Text;

namespace OllamaCommitGen.Cli.Utils;

public static class ConsoleWrapper
{
    public static string WriteEditableLine(string text)
    {
        Console.Write("\x1B[5 q");
        var initPos = Console.GetCursorPosition();
        Console.Write(text);
        var sb = new StringBuilder(text);
        var sbPos = Console.GetCursorPosition();
        ConsoleKeyInfo keyInfo;

        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            var pos = Console.GetCursorPosition();

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow when pos.Left > initPos.Left || pos.Top > initPos.Top:
                    if (pos.Top > initPos.Top)
                    {
                        if (pos.Left - 1 >= 0)
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        else
                            Console.SetCursorPosition(Console.BufferWidth - 1, pos.Top - 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                    }

                    break;
                case ConsoleKey.RightArrow when pos.Left < sbPos.Left || pos.Top < sbPos.Top:
                    if (pos.Top < sbPos.Top)
                    {
                        if (pos.Left + 1 < Console.BufferWidth)
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        else
                            Console.SetCursorPosition(0, pos.Top + 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(pos.Left + 1, pos.Top);
                    }
                    
                    break;
                case ConsoleKey.Home:
                    Console.SetCursorPosition(initPos.Left, initPos.Top);
                    break;
                case ConsoleKey.End:
                    Console.SetCursorPosition(sbPos.Left, sbPos.Top);
                    break;
                case ConsoleKey.Backspace when pos.Left > initPos.Left || pos.Top > initPos.Top:
                    sb = sb.Remove((pos.Top - initPos.Top) * Console.BufferWidth - initPos.Left + pos.Left - 1, 1);
                    ClearFromTo(initPos, sbPos);
                    Console.Write(sb.ToString());
                    sbPos = Console.GetCursorPosition();

                    if (pos.Top > initPos.Top)
                    {
                        if (pos.Left - 1 >= 0)
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        else
                            Console.SetCursorPosition(Console.BufferWidth - 1, pos.Top - 1);
                    }
                    else
                    {
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                    }

                    break;
                case ConsoleKey.Delete when pos.Left != sbPos.Left || pos.Top != sbPos.Top:
                    sb = sb.Remove((pos.Top - initPos.Top) * Console.BufferWidth - initPos.Left + pos.Left, 1);
                    ClearFromTo(initPos, sbPos);
                    Console.Write(sb.ToString());
                    sbPos = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos.Left, pos.Top);
                    break;
                default:
                    if (char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsPunctuation(keyInfo.KeyChar) ||
                        keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        sb = sb.Insert((pos.Top - initPos.Top) * Console.BufferWidth - initPos.Left + pos.Left, keyInfo.KeyChar);
                        ClearFromTo(initPos, sbPos);
                        Console.Write(sb.ToString());
                        sbPos = Console.GetCursorPosition();
                        if (pos.Top < sbPos.Top)
                        {
                            if (pos.Left + 1 < Console.BufferWidth)
                                Console.SetCursorPosition(pos.Left + 1, pos.Top);
                            else
                                Console.SetCursorPosition(0, pos.Top + 1);
                        }
                        else if (pos.Left < sbPos.Left)
                        {
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        }
                    }

                    break;
            }
        }

        Console.SetCursorPosition(sbPos.Left, sbPos.Top);
        Console.Write("\x1B[0 q");
        Console.Write(Environment.NewLine);

        return sb.ToString();
    }

    public static void ClearFromTo((int left, int top) pos1, (int left, int top) pos2)
    {
        Console.SetCursorPosition(pos2.left, pos2.top);

        while (pos1.left != pos2.left || pos1.top != pos2.top)
            if (pos2.left == 0 && pos2.top > pos1.top)
            {
                pos2.left = Console.BufferWidth - 1;
                pos2.top -= 1;
                Console.SetCursorPosition(pos2.left, pos2.top);
                Console.Write(" ");
                Console.SetCursorPosition(pos2.left, pos2.top);
            }
            else
            {
                Console.Write("\b \b");
                Console.SetCursorPosition(--pos2.left, pos2.top);
            }

        Console.SetCursorPosition(pos1.left, pos1.top);
    }
}