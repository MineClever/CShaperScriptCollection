using System;

class Program
{
    public static void ScriptMain(string[] args)
    {
        Console.WriteLine("Hello, CSX scripting!");
        int result = Add(5, 3);
        Console.WriteLine($"Result: {result}");
    }

    public static int Add(int a, int b)
    {
        return a + b;
    }
}

// 调用入口函数
Program.ScriptMain(null);