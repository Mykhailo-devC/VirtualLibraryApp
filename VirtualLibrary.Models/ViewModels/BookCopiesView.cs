namespace VirtualLibrary.Models.ViewModels
{
    public class BookCopiesView
    {
        public int CopyId { get; set; }

        public int Isbn { get; set; }

        public BooksView Book { get; set; } = null!;

        public ItemsView Items { get; set; } = null!;

    }
}
