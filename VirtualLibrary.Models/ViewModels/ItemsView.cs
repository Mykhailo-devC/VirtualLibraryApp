namespace VirtualLibrary.Models.ViewModels
{
    public class ItemsView
    {
        public int Id { get; set; }

        public DateTime PublishDate { get; set; }

        public PublishersView Publishers { get; set; } = null!;
    }
}