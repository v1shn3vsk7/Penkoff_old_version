namespace Penkoff_ASP.NET_Core_;

public class SetCurrency
{
    public static string Print(string str)
    {
        if (str.Length < 4) return str;

        for (int i = str.Length - 3; i > 0; i -= 3)
        {
            str = str.Insert(i, " ");
        }

        return str;
    }
}

