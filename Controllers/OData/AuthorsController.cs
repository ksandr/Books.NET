using System.Linq;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Controllers.OData
{
    public class AuthorsController : BaseODataController
    {
        public AuthorsController(BooksContext db, ILogger<AuthorsController> logger)
            : base(db, logger)
        { }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(db.Authors);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(db.Authors.FirstOrDefault(x => x.Id == key));
        }

        [EnableQuery]
        public IActionResult Books(int key)
        {
            return Ok(db.QueryBooks().Where(x => x.AuthorList.Any(a => a.AuthorId == key)));
        }
    }
}
