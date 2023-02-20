namespace KnowledgeBaseForum.Commons.Utils
{
    /// <summary>
    /// Implements operations in which data is compared without regards to specific conditions.
    /// E.g.: Strings compared regardless of culture and casing.
    /// </summary>
    public static class InvariantComparison
    {
        /// <summary>
        /// Compares two strings and return if they are equal, despite casing.
        /// </summary>
        /// <param name="inputA">The first string.</param>
        /// <param name="inputB">The second string.</param>
        /// <returns><c>true</c> if they are equal, <c>false</c> otherwise.</returns>
        public static bool InvariantEquals(this string inputA, string inputB) => inputA.ToUpperInvariant().Equals(inputB.ToUpperInvariant());

        /// <summary>
        /// Compares two strings and return if the first contains the second, disregarding casing.
        /// </summary>
        /// <param name="sourceA">The first string.</param>
        /// <param name="sourceB">The second string.</param>
        /// <returns><c>true</c> if the second exists within the first, <c>false</c> otherwise.</returns>
        public static bool InvariantContains(this string sourceA, string sourceB) => sourceA.ToUpperInvariant().Contains(sourceB.ToUpperInvariant());
    }
}
