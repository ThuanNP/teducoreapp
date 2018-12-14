using System;

namespace TeduCoreApp.Utilities.Dtos
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount
        {
            get => (int)Math.Ceiling((double)RowCount / PageSize);
            set => PageCount = value;
        }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }
}
