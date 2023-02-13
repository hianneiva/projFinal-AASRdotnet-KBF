namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Miscellaneous operations to be used app wide.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Compares two strings and return if they are equal, despite casing.
        /// </summary>
        /// <param name="sourceA">The first string.</param>
        /// <param name="sourceB">The second string.</param>
        /// <returns><c>true</c> if they are equal, <c>false</c> otherwise.</returns>
        public static bool InvariantEquals(this string sourceA, string sourceB) => sourceA.ToUpperInvariant().Equals(sourceB.ToUpperInvariant());

        /// <summary>
        /// Compares two strings and return if the first contains the second, disregarding casing.
        /// </summary>
        /// <param name="sourceA">The first string.</param>
        /// <param name="sourceB">The second string.</param>
        /// <returns><c>true</c> if the second exists within the first, <c>false</c> otherwise.</returns>
        public static bool InvariantContains(this string sourceA, string sourceB) => sourceA.ToUpperInvariant().Contains(sourceB.ToUpperInvariant());
    }
}
