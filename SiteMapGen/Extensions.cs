namespace SiteMapGen
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the specified elements contains any.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>
        ///   <c>true</c> if the specified elements contains any; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAny(this string str, params string [] elements)
        {
            foreach (var e in elements)
                if (str.Contains(e))
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether the specified elements contains all.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>
        ///   <c>true</c> if the specified elements contains all; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAll(this string str, params string[] elements)
        {
            foreach (var e in elements)
                if (!str.Contains(e))
                    return false;

            return true;
        }
    }
}
