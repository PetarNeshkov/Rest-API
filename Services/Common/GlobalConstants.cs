namespace Common;

public static class GlobalConstants
{
    public const string SuccessfulCreationMessage = "The {0} was successfully created.";
    public const string SuccessfulEditMessage = "The {0} was successfully edited.";
    public const string SuccessfulDeleteMessage = "The {0} was successfully deleted.";
    public const int BooksPerPage = 10;
    public static class ErrorMessages
    {
        public const string IdModelIdValidationMessage = "The provided id for the entity is invalid.";
        public const string BookDoesNotExistMessage = "Book does not exist.";
        public const string FailedCreationMessage = "Creation failed.";
        public const string InvalidPageNumberMessage = "Page number must be 1 or greater.";
        public static class Books
        {
            public const string ExistsMessage = "Book already exists";
            public const string TitleIsRequiredMessage = "Book title is required.";
            public const string TitleMaxLengthMessage = "Book title cannot exceed {0} characters.";
            public const string AuthorIsRequiredMessage = "At least one author is required.";
            public const string AuthorMaxLengthMessage = "Book author cannot exceed {0} characters.";
            public const string GenreIsRequiredMessage = "At least one author is required.";
            public const string GenreMaxLengthRequiredMessage = "Book genre cannot exceed {0} characters.";
            public const string PublishedYearRequiredMessage = "Book published year is required.";
            public const string NotFoundMessage = "Book not found.";
        }
    }
}