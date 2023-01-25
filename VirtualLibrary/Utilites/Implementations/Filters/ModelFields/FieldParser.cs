namespace VirtualLibrary.Utilites.Implementations.Filters.ModelFields
{
    public static class FieldParser
    {
        public static Dictionary<ModelField, Func<Publisher, object>> PublisherFields = new Dictionary<ModelField, Func<Publisher, object>>
        {
            {ModelField.Name, x => x.Name }
        };

        public static Dictionary<ModelField, Func<Book, object>> BookFields = new Dictionary<ModelField, Func<Book, object>>
        {
            {ModelField.Name, x => x.Name },
            {ModelField.Author, x => x.Author },
            {ModelField.Isbn, x => x.BookCopies.Min(a => a.Isbn)},
            {ModelField.PublishDate, x => x.BookCopies.Min(a => a.Item.PublishDate) },
            {ModelField.Publisher, x => x.BookCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static Dictionary<ModelField, Func<Article, object>> ArticleFields = new Dictionary<ModelField, Func<Article, object>>
        {
            {ModelField.Name, x => x.Name },
            {ModelField.Author, x => x.Author },
            {ModelField.Version, x => x.ArticleCopies.Min(a => a.Version)},
            {ModelField.PublishDate, x => x.ArticleCopies.Min(a => a.Item.PublishDate) },
            {ModelField.Publisher, x => x.ArticleCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static Dictionary<ModelField, Func<Magazine, object>> MagazineFields = new Dictionary<ModelField, Func<Magazine, object>>
        {
            {ModelField.Name, x => x.Name },
            {ModelField.IssueNumber, x => x.MagazineCopies.Min(a => a.IssureNumber)},
            {ModelField.PublishDate, x => x.MagazineCopies.Min(a => a.Item.PublishDate) },
            {ModelField.Publisher, x => x.MagazineCopies.Min(a => a.Item.Publisher.Name) }
        };

        public static bool TryParseField(string fieldName, out ModelField parsedField)
        {
            if(Enum.TryParse(fieldName, true, out parsedField))
            {
                return true;
            }
            else
            {
                parsedField = ModelField.Undefined;
                return false;
            }
        }
    }
}
