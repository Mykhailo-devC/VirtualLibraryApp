namespace VirtualLibrary.Utilites.Implementations.Filters.ModelFields
{
    public static class FieldParser
    {
        public static Dictionary<ModelFields, Func<Publisher, object>> PublisherFields = new Dictionary<ModelFields, Func<Publisher, object>>
        {
            {ModelFields.Name, x => x.Name }
        };

        public static Dictionary<ModelFields, Func<Book, object>> BookFields = new Dictionary<ModelFields, Func<Book, object>>
        {
            {ModelFields.Name, x => x.Name },
            {ModelFields.Author, x => x.Author },
            {ModelFields.Isbn, x => x.BookCopies.Min(a => a.Isbn)},
            {ModelFields.PublishDate, x => x.BookCopies.Min(a => a.Item.PublishDate) },
            {ModelFields.Publisher, x => x.BookCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static Dictionary<ModelFields, Func<Article, object>> ArticleFields = new Dictionary<ModelFields, Func<Article, object>>
        {
            {ModelFields.Name, x => x.Name },
            {ModelFields.Author, x => x.Author },
            {ModelFields.Version, x => x.ArticleCopies.Min(a => a.Version)},
            {ModelFields.PublishDate, x => x.ArticleCopies.Min(a => a.Item.PublishDate) },
            {ModelFields.Publisher, x => x.ArticleCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static Dictionary<ModelFields, Func<Magazine, object>> MagazineFields = new Dictionary<ModelFields, Func<Magazine, object>>
        {
            {ModelFields.Name, x => x.Name },
            {ModelFields.IssueNumber, x => x.MagazineCopies.Min(a => a.IssureNumber)},
            {ModelFields.PublishDate, x => x.MagazineCopies.Min(a => a.Item.PublishDate) },
            {ModelFields.Publisher, x => x.MagazineCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static bool TryParseField(string fieldName, out ModelFields parsedField)
        {
            if(Enum.TryParse(fieldName, true, out parsedField))
            {
                return true;
            }
            else
            {
                parsedField = ModelFields.Undefined;
                return false;
            }
        }
    }
}
