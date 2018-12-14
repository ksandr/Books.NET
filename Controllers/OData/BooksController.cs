using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using Books.Utils;
using Ksandr.Books.Database;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ksandr.Books.Controllers.OData
{
    public class BooksController : BaseODataController
    {
        private readonly string _libraryPath;

        public BooksController(BooksContext db, ILogger<BooksController> logger, IConfiguration config)
            : base(db, logger)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _libraryPath = config.GetSection("AppConfig:LibraryPath").Get<string>();
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(db.Books);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(db.Books.Include(x => x.Series).Include(x => x.Authors).Include(x => x.Genres).FirstOrDefault(x => x.Id == key));
        }

        public IActionResult Download(int key)
        {
            bool returnZip = Request.Query.Any(x => x.Key == "zip");

            Book book = db.Books.SingleOrDefault(x => x.Id == key);
            if (book == null)
                return NotFound();

            byte[] fileContent = ReadFileContent(book);
            if (fileContent == null)
                return NotFound();

            if (returnZip)
            {
                FileContentResult zipResult = new FileContentResult(WriteZipFile(book.Title + book.Ext, fileContent), "application/zip")
                {
                    FileDownloadName = book.FileName + ".zip",
                };

                return zipResult;
            }

            // TODO: Select content type by book.Ext
            FileContentResult fb2Result = new FileContentResult(fileContent, "application/x-fictionbook")
            {
                FileDownloadName = book.FileName + book.Ext,
            };

            return fb2Result;
        }

        private byte[] ReadFileContent(Book book)
        {
            string zipName = Path.Combine(_libraryPath, book.Folder);
            if (!System.IO.File.Exists(zipName))
                return null;

            return ReadZipEntry(zipName, book.FileName + book.Ext);
        }

        private byte[] ReadZipEntry(string zipName, string fileName)
        {
            using (ZipArchive zip = ZipFile.OpenRead(zipName))
            {
                ZipArchiveEntry entry = zip.GetEntry(fileName);

                if (entry == null)
                    return null;

                return entry.Open().ReadBytes();
            }
        }

        private byte[] WriteZipFile(string fileName, byte[] fileContent)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry xmlEntry = archive.CreateEntry(fileName);
                    using (BinaryWriter writer = new BinaryWriter(xmlEntry.Open()))
                    {
                        writer.Write(fileContent);
                    }
                }

                return ms.ToArray();
            }
        }

        public IActionResult Details(int key)
        {

            Book book = db.Books.SingleOrDefault(x => x.Id == key);
            if (book == null)
                return NotFound();

            byte[] fileContent = ReadFileContent(book);
            if (fileContent == null)
                return NotFound();

            bool hasBOM = fileContent[0] == 0xEF && fileContent[1] == 0xBB && fileContent[2] == 0xBF;
            if (hasBOM)
                fileContent = fileContent.Skip(3).ToArray();
            string fileText = Encoding.UTF8.GetString(fileContent);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(fileText);

            XmlDeclaration declaration = xml.FirstChild as XmlDeclaration;
            if (declaration != null && !string.Equals(declaration.Encoding, "utf-8", StringComparison.InvariantCultureIgnoreCase))
            {
                Encoding encoding = CodePagesEncodingProvider.Instance.GetEncoding(declaration.Encoding);
                fileText = encoding.GetString(fileContent);
                xml.LoadXml(fileText);
            }

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("fb2", @"http://www.gribuser.ru/xml/fictionbook/2.0");
            nsmgr.AddNamespace("l", @"http://www.w3.org/1999/xlink");

            (byte[] image, string contentType) = GetCover(xml, nsmgr);
            string annotation = GetAnnotation(xml, nsmgr);

            return new JsonResult(new { Annotation = annotation, Cover = new { Data = image, ContentType = contentType } });
        }

        private (byte[] image, string contentType) GetCover(XmlDocument xml, XmlNamespaceManager nsmgr)
        {
            XmlNode coverNode = xml.DocumentElement
                .SelectSingleNode(@"/fb2:FictionBook/fb2:description/fb2:title-info/fb2:coverpage/fb2:image", nsmgr);
            if (coverNode == null)
                return (null, null);

            XmlAttribute hrefAttribute = coverNode.Attributes["href", @"http://www.w3.org/1999/xlink"];
            if (hrefAttribute == null)
                return (null, null);

            string imageId = hrefAttribute.Value.TrimStart('#');
            XmlNodeList images = xml.DocumentElement.SelectNodes($"//fb2:binary[@id='{imageId}']", nsmgr);
            if (images.Count == 0)
                return (null, null);

            string base64 = images[0].InnerText;
            string contentType = images[0].Attributes["content-type"]?.Value;

            return (Convert.FromBase64String(base64), contentType);
        }

        private string GetAnnotation(XmlDocument xml, XmlNamespaceManager nsmgr)
        {
            XmlNode annotationNode = xml.DocumentElement
                .SelectSingleNode(@"/fb2:FictionBook/fb2:description/fb2:title-info/fb2:annotation", nsmgr);

            if (annotationNode == null)
                return null;

            string annotation = annotationNode.OuterXml
                .Replace("<annotation xmlns=\"http://www.gribuser.ru/xml/fictionbook/2.0\">", "")
                .Replace("</annotation>", "");

            return annotation;
        }
    }
}
