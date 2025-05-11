using Microsoft.EntityFrameworkCore;

namespace WebApi.DbOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using ( var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return ;
                }
                context.Books.AddRange( 

                    new Book{Title="beyazDis",PageCount=300,GenreId=1,PublishDate=new DateTime(1998,06,18)},
                    new Book{Title="olimposlular",PageCount=200,GenreId=2,PublishDate=new DateTime(2001,06,18)},
                    new Book{Title="satranc",PageCount=250,GenreId=2,PublishDate=new DateTime(2001,06,18)},
                    new Book{Title="insancıklar",PageCount=350,GenreId=3,PublishDate=new DateTime(2002,06,18)},
                    new Book{Title="od",PageCount=320,GenreId=1,PublishDate=new DateTime(2005,06,18)},
                    new Book{Title="sadfd",PageCount=123,GenreId=1,PublishDate=new DateTime(2003,03,03)}
                    );
                    context.SaveChanges();
            }
        }
    }
}