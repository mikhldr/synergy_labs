namespace Part2_Repository.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Year { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
