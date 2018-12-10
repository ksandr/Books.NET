using System.Linq;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(db.Genres);
        }

        [EnableQuery]
        public IActionResult Get(string key)
        {
            return Ok(db.Genres.FirstOrDefault(x => x.Id == key));
        }

        [EnableQuery]
        public IActionResult Books(string key)
        {
            return Ok(db.QueryBooks().Where(x => x.Genres.Any(g => g.Id == key)));
        }
    }
}
