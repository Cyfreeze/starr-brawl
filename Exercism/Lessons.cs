using System;
using System.Text;

public static class Lessons
{
    public static void Foo()
    {
        string s = "";
        for (int i = 0; i < 1_000; i++)
        {
            s += i.ToString(); // Inefficient
        }

        // This loop above leads to poor performance.

        // Instead of doing this above we can simply use the StringBuilder:
        StringBuilder sb = new StringBuilder();

        sb.Append("Hello");
        sb.Append(" ");
        sb.Append("World!");

        Console.WriteLine(sb.ToString()); // output: Hello World!

        string wefwelkflwqekflwekf = "Emircan";
        Console.WriteLine(wefwelkflwqekflwekf[6]); // output: n
    }
}
/*

Primitive Type examples: int, bool, byte, char, float, double, string
Referenced Type examples: Arrays, Strings, Classes/Objects, Interfaces, Collections, etc...

    - Strings are essentially a collection of chars (characters).
    - Strings are between Primitive and Referenced types nearly in all languages.
    - Strings are immutable objects: once created, their content cannot be changed.
        string s = "Hello"; // 6 bytes
        s = "Hello World!"; // 13 bytes + 6 bytes = 19 bytes
    - This code flow above, creates a new string in memory, which is essentially inefficient.
    - Especially in loops, creating a new string again and again in every loop iteration is so much work and place in memory.

StringBuilder:

    - StringBuilder is mutable, meaning you can change the content of the string without creating a new object each time.
    - StringBuilder's improvements:
        1. Performance: Greatly improves performance when modifying strings frequently.
        2. Memory Efficieny/Optimisation: Reduces the number of intermediate string objects.
        3. Ease of use: Has handy methods for appending, inserting, deleting, etc.

*/


public static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int max)
    {
        int iSumme = 0;

        for (int i = 0; i <= max; i++)
        {
            iSumme += i;
        }

        return iSumme * iSumme;
    }

    public static int CalculateSumOfSquares(int max)
    {
        int iSumme = 0;

        for (int i = 0; i <= max; i++)
        {
            iSumme += i * i;
        }

        return iSumme;
    }

    // This function is a lambda function thus we cannot break line
    public static int CalculateDifferenceOfSquares(int max) => Math.Abs(CalculateSquareOfSum(max) - CalculateSumOfSquares(max));
}

// Math.Abs: (absolute): this function gives the absolute numeric value
// |2-4| = |-2|= 2
// |4-3| = |1| = 1