namespace TestTaskUkad.Models
{
    public static class Extensions
    {
        public static bool ContainsAny(this string str, params string [] elements)
        {
            foreach (var e in elements)
                if (str.Contains(e))
                    return true;

            return false;
        }

        public static bool ContainsAll(this string str, params string[] elements)
        {
            foreach (var e in elements)
                if (!str.Contains(e))
                    return false;

            return true;
        }
    }
}
