using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Bookkeeping.Models;
using Microsoft.AspNetCore.Http;

namespace Bookkeeping.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost("test")]
        public IActionResult test(string hej)
        {
            return Ok();
        }

        [HttpPost("ProcessFiles")]
        public async Task<IActionResult> ProcessFilesAsync()
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                var filePath = Path.GetTempPath();

                var swishFilePath = Path.Combine(filePath, "swish_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".txt");
                var transFilePath = Path.Combine(filePath, "trans_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".txt");

                foreach (var file in files)
                {
                    if (file.Length > 0 && file.ContentType == "text/plain")
                    {
                        var path = "";
                        if (file.FileName == "Swishrapport.txt")
                        {
                            path = swishFilePath;
                        }
                        else if (file.FileName == "Transaktionsrapport.txt")
                        {
                            path = transFilePath;
                        }
                        else
                        {
                            return BadRequest(new { message = "Unknown file" });
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Empty file or wrong filetype" });
                    }
                }

                var swishLines = System.IO.File.ReadAllLines(swishFilePath, Encoding.GetEncoding("ISO-8859-1"));
                var transLines = System.IO.File.ReadAllLines(transFilePath, Encoding.GetEncoding("ISO-8859-1"));

                var rows = new List<Row>();

                for (int i = 2; i < swishLines.Length; i++)
                {
                    var strParts = new List<string>(swishLines[i].Split('\t')).Where(t => !String.IsNullOrWhiteSpace(t));

                    var row = new Row
                    {
                        RegisterDate = DateTime.Parse(strParts.ElementAt(3)),
                        TransactionDate = DateTime.Parse(strParts.ElementAt(4) + " " + strParts.ElementAt(10)),
                        CurrencyDate = DateTime.Parse(strParts.ElementAt(5)),
                        Amount = decimal.Parse(strParts.ElementAt(11)),
                        Reference = strParts.ElementAt(9),
                        PhoneNumber = strParts.ElementAt(8),
                        TransactionType = "SWISH"
                    };

                    rows.Add(row);
                }

                for (int i = 2; i < transLines.Length; i++)
                {
                    var strParts = new List<string>(transLines[i].Split('\t')).Where(t => !String.IsNullOrWhiteSpace(t));

                    var row = new Row
                    {
                        RegisterDate = DateTime.Parse(strParts.ElementAt(5)),
                        TransactionDate = DateTime.Parse(strParts.ElementAt(6)),
                        CurrencyDate = DateTime.Parse(strParts.ElementAt(7)),
                        Amount = decimal.Parse(strParts.ElementAt(10)),
                        Reference = strParts.ElementAt(8),
                        TransactionType = strParts.ElementAt(9),
                    };

                    if (!row.TransactionType.StartsWith("Swish"))
                    {
                        rows.Add(row);
                    }
                }

                rows = rows.OrderBy(x => x.RegisterDate).ToList();

                return Ok(rows);
            }else
            {
                return BadRequest(new { message = "No files uploaded" });
            }
        }
    }
}
