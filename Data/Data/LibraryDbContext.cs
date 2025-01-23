using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) 
    : DbContext(options)
{
    public DbSet<Book> Books { get; init; }
}