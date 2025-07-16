using System;
using System.Collections.Generic;

public static class Isogram
{
    public static bool IsIsogram(string word)
    {
        // 1. Pre-conditions
        if (word == null || word == "")
        {
            return true;
        }

        HashSet<char> seen = new HashSet<char>();
        foreach (char c in word.ToLower())
        {
            if (char.IsLetter(c))
            {
                if (seen.Contains(c))
                {
                    return false;
                }
                else
                {
                    seen.Add(c);
                }
            }
        }
        return true;
    }
}

/*

A HashSet<T> is a collection in C# that stores unique elements only, meaning no duplicates are allowed.
It is unordered and is based on a hash table.
Basing on hash table allows for fast lookups, insertions, and deletions.

When to use HashSet<T>
- You need a collection of unique items.
- You need fast lookup for Contains.
- You don't care about the order of elements.

*/