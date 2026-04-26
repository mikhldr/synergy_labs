using Repository.Data;
using Repository.Entities;
using Repository.Repositories;

Console.OutputEncoding = System.Text.Encoding.UTF8;
// test comment for push
using var ctx = new BookshopContext();
ctx.Database.EnsureCreated();

using var uow = new UnitOfWork(ctx);

if (!uow.Authors.GetAll().Any())
{
    var pushkin = new Author { FullName = "А. С. Пушкин", Country = "Россия" };
    var tolkien = new Author { FullName = "Дж. Р. Р. Толкин", Country = "Великобритания" };
    uow.Authors.Add(pushkin);
    uow.Authors.Add(tolkien);

    uow.Books.Add(new Book { Title = "Евгений Онегин", Price = 250m, Year = 1833, AuthorId = pushkin.Id });
    uow.Books.Add(new Book { Title = "Капитанская дочка", Price = 180m, Year = 1836, AuthorId = pushkin.Id });
    uow.Books.Add(new Book { Title = "Властелин колец", Price = 950m, Year = 1954, AuthorId = tolkien.Id });
    uow.Books.Add(new Book { Title = "Хоббит", Price = 420m, Year = 1937, AuthorId = tolkien.Id });
}

Console.WriteLine("Все книги");
foreach (var b in uow.Books.GetAll())
    Console.WriteLine($"  #{b.Id} {b.Title} ({b.Year}) — {b.Price} руб.");

Console.WriteLine("\nПоиск по автору (id=1)");
foreach (var b in uow.Books.GetByAuthor(1))
    Console.WriteLine($"  {b.Title}");

Console.WriteLine("\nКниги в диапазоне 200..500 руб.");
foreach (var b in uow.Books.GetByPriceRange(200m, 500m))
    Console.WriteLine($"  {b.Title} — {b.Price} руб.");

Console.WriteLine("\nПоиск по слову 'кол'");
foreach (var b in uow.Books.SearchByTitle("кол"))
    Console.WriteLine($"  {b.Title}");

Console.WriteLine("\nАсинхронное получение");
var allAsync = await uow.Books.GetAllAsync();
Console.WriteLine($"  Всего книг (async): {allAsync.Count()}");

Console.WriteLine("\nГотово.");
