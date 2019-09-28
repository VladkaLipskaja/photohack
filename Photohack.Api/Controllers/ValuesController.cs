using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Photohack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //            static void Main(string[] args)
            //            {
            //                var dtContent = GetDataTableFromExcel(@”c:\temp\test.xlsx”);
            //                //var res = from DataRow dr in dtContent.Rows
            //                // where (string)dr[“Name”] == “Gil”
            //                // select ((string)dr[“Section”]).FirstOrDefault();
            //                foreach (DataRow dr in dtContent.Rows)
            //                {
            //                    Console.WriteLine(dr[“Name”].ToString());
            //                }
            //                Console.ReadLine();
            //            }
            //            private static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
            //            {
            //                using (var pck = new OfficeOpenXml.ExcelPackage())
            //                {
            //                    using (var stream = File.OpenRead(path))
            //                    {
            //                        pck.Load(stream);
            //                    }
            //                    var ws = pck.Workbook.Worksheets.First();
            //                    DataTable tbl = new DataTable();
            //                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            //                    {
            //                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format(“Column { 0}”, firstRowCell.Start.Column));
            //        }
            //        var startRow = hasHeader ? 2 : 1;
            //for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            //{
            //var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
            //        DataRow row = tbl.Rows.Add();
            //foreach (var cell in wsRow)
            //{
            //row[cell.Start.Column — 1] = cell.Text;
            //}
            //}
            //return tbl;
            //}
            return new string[] { "v", "v1" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
