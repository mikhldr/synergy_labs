namespace Part2_Repository.Entities;

public class Author
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = new();
}
