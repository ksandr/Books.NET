using System;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Controllers.OData
{
    public abstract class BaseODataController : ODataController
    {
        protected readonly ILogger<BaseODataController> logger;
        protected readonly BooksContext db;

        public BaseODataController(BooksContext db, ILogger<BaseODataController> logger)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
