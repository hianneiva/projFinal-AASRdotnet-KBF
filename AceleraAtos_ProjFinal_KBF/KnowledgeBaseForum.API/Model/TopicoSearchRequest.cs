namespace KnowledgeBaseForum.API.Model
{
    public class TopicoSearchRequest
    {
        /// <summary>
        /// Filter when searching topic title.
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Filter when searching author.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Tags to be considered in the search.
        /// </summary>
        public IEnumerable<string>? Tags { get; set; }

        /// <summary>
        /// Search results in recent date order.
        /// </summary>
        public bool Recent { get; set; }

        /// <summary>
        /// Search results in alphabetic order.
        /// </summary>
        public bool Alphabetic { get; set; }
    }
}
