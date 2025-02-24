

namespace PropertyManagement.Core.DTOs.Shared;
public class PaginatedRequest
    {
        /// <summary>
        /// The page number to retrieve.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of records per page.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// The search term for filtering results by name, email, or other details.
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// The index of the column to sort by.
        /// </summary>
        public int OrderColumnIndex { get; set; } = 0;

        /// <summary>
        /// The direction of sorting ('ASC' or 'DESC').
        /// </summary>
        public string OrderDirection { get; set; } = "ASC";
}

