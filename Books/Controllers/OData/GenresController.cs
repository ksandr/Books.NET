using System.Linq;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Controllers.OData
{
    public class GenresController : BaseODataController
    {
        public GenresController(BooksContext db, ILogger<GenresController> logger)
            : base(db, logger)
        { }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(db.Genres.AsNoTracking());
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(db.Genres.AsNoTracking().FirstOrDefault(x => x.Id == key));
        }

        [EnableQuery]
        public IActionResult Books(string key)
        {
            return Ok(db.Books.AsNoTracking().Where(x => x.Genres.Any(g => g.Id == key)));
        }
    }
}
