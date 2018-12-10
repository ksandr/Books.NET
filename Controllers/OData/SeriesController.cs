using System.Linq;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Controllers.OData
{
    public class SeriesController : BaseODataController
    {
        public SeriesController(BooksContext db, ILogger<SeriesController> logger)
            : base(db, logger)
        { }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(db.Series);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(db.Series.FirstOrDefault(x => x.Id == key));
        }

        [EnableQuery]
        public IActionResult Books(int key)
        {
            return Ok(db.QueryBooks().Where(x => x.Series.Id == key));
        }
    }
}
