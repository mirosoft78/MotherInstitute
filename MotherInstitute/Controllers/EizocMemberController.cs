using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace MotherInstitute.Controllers
{
    public class EizocMemberController : Controller
    {
        private readonly IConfiguration _configuration;

        public EizocMemberController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ================= INDEX =================

        public IActionResult Index()
        {
            DataTable dt = new DataTable();

            string connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"
            SELECT TOP 100 *
            FROM STUDENTREGD
            ORDER BY SLNO ASC
        ";

                using (SqlCommand cmd =
                    new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return View(dt);
        }
        // ================= UPLOAD EXCEL =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadExcel(IFormFile excelFile)
        {
            DataTable dt = new DataTable();

            try
            {
                if (excelFile == null || excelFile.Length == 0)
                {
                    TempData["ErrorMessage"] =
                        "Please select file.";

                    return RedirectToAction("Index");
                }

                string extension =
                    Path.GetExtension(excelFile.FileName)
                        .ToLower();

                // ================= EXCEL FILE =================

                if (extension == ".xlsx" || extension == ".xls")
                {
                    using (var stream = new MemoryStream())
                    {
                        excelFile.CopyTo(stream);

                        using (var workbook =
                            new XLWorkbook(stream))
                        {
                            var worksheet =
                                workbook.Worksheet(1);

                            var rows =
                                worksheet.RangeUsed()
                                         .RowsUsed();

                            bool firstRow = true;

                            foreach (var row in rows)
                            {
                                // HEADER

                                if (firstRow)
                                {
                                    foreach (var cell in row.Cells())
                                    {
                                        string columnName =
                                            cell.Value
                                                .ToString()
                                                .Trim();

                                        if (!dt.Columns.Contains(columnName))
                                        {
                                            dt.Columns.Add(columnName);
                                        }
                                    }

                                    firstRow = false;
                                }

                                // DATA

                                else
                                {
                                    DataRow dr =
                                        dt.NewRow();

                                    for (int i = 0;
                                         i < dt.Columns.Count;
                                         i++)
                                    {
                                        dr[i] =
                                            row.Cell(i + 1)
                                               .Value
                                               .ToString();
                                    }

                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }

                // ================= CSV FILE =================

                else if (extension == ".csv")
                {
                    using (TextFieldParser parser =
                        new TextFieldParser(
                            excelFile.OpenReadStream()))
                    {
                        parser.TextFieldType =
                            FieldType.Delimited;

                        parser.SetDelimiters(",");

                        parser.HasFieldsEnclosedInQuotes = true;

                        bool firstRow = true;

                        while (!parser.EndOfData)
                        {
                            string[] fields =
                                parser.ReadFields();

                            if (fields == null ||
                                fields.Length == 0)
                            {
                                continue;
                            }

                            // HEADER

                            if (firstRow)
                            {
                                foreach (string header in fields)
                                {
                                    string columnName =
                                        header.Trim()
                                              .Replace("\"", "");

                                    if (!dt.Columns.Contains(columnName))
                                    {
                                        dt.Columns.Add(columnName);
                                    }
                                }

                                firstRow = false;
                            }

                            // DATA

                            else
                            {
                                DataRow dr =
                                    dt.NewRow();

                                for (int i = 0;
                                     i < dt.Columns.Count;
                                     i++)
                                {
                                    if (i < fields.Length)
                                    {
                                        dr[i] =
                                            fields[i]
                                            .Trim()
                                            .Replace("\"", "");
                                    }
                                    else
                                    {
                                        dr[i] = "";
                                    }
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                else
                {
                    TempData["ErrorMessage"] =
                        "Only Excel or CSV file allowed.";

                    return RedirectToAction("Index");
                }

                // ================= DATABASE INSERT =================

                string connectionString =
                    _configuration.GetConnectionString(
                        "DefaultConnection");

                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    con.Open();

                    // GET DATABASE COLUMNS

                    List<string> dbColumns =
                        new List<string>();

                    string columnQuery = @"
                        SELECT COLUMN_NAME
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = 'STUDENTREGD'
                    ";

                    using (SqlCommand columnCmd =
                        new SqlCommand(columnQuery, con))
                    {
                        using (SqlDataReader reader =
                            columnCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dbColumns.Add(
                                    reader["COLUMN_NAME"]
                                    .ToString()
                                    .Trim()
                                    .ToUpper());
                            }
                        }
                    }

                    // ================= INSERT ROWS =================

                    int successCount = 0;

                    int failCount = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            List<string> matchedColumns =
                                new List<string>();

                            List<string> matchedParams =
                                new List<string>();

                            SqlCommand cmd =
                                new SqlCommand();

                            cmd.Connection = con;

                            foreach (DataColumn col in dt.Columns)
                            {
                                string columnName =
                                    col.ColumnName.Trim();

                                if (dbColumns.Contains(
                                    columnName.ToUpper()))
                                {
                                    matchedColumns.Add(
                                        columnName);

                                    string parameterName =
                                        "@" + columnName;

                                    matchedParams.Add(
                                        parameterName);

                                    string value =
                                        row[columnName]
                                        ?.ToString()
                                        ?.Trim();

                                    if (string.IsNullOrWhiteSpace(value))
                                    {
                                        value = "";
                                    }

                                    // PREVENT TRUNCATION

                                    if (value.Length > 100)
                                    {
                                        value =
                                            value.Substring(0, 100);
                                    }

                                    cmd.Parameters.AddWithValue(
                                        parameterName,
                                        value);
                                }
                            }

                            // REQUIRED AUTO COLUMNS

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "DOR",
                                "@DOR",
                                DateTime.Now);

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "STUDENTID",
                                "@STUDENTID",
                                Guid.NewGuid()
                                .ToString()
                                .Substring(0, 10));

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "GENDER",
                                "@GENDER",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "CASTE",
                                "@CASTE",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "AADHARNO",
                                "@AADHARNO",
                                "000000000000");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "BLOODGROUP",
                                "@BLOODGROUP",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "MOB1",
                                "@MOB1",
                                "0000000000");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "MOB2",
                                "@MOB2",
                                "0000000000");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "NAME",
                                "@NAME",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "FNAME",
                                "@FNAME",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "MNAME",
                                "@MNAME",
                                "NA");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "CURRYR",
                                "@CURRYR",
                                "2025");

                            AddDefaultColumn(
                                cmd,
                                matchedColumns,
                                matchedParams,
                                "DOB",
                                "@DOB",
                                DateTime.Now);

                            // INSERT

                            if (matchedColumns.Count > 0)
                            {
                                string insertQuery = $@"
                                    INSERT INTO STUDENTREGD
                                    ({string.Join(",", matchedColumns)})
                                    VALUES
                                    ({string.Join(",", matchedParams)})
                                ";

                                cmd.CommandText =
                                    insertQuery;

                                cmd.ExecuteNonQuery();

                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            failCount++;

                            TempData["RowError"] =
                                "Failed Row Data: " +
                                string.Join(" | ", row.ItemArray) +
                                " Error: " +
                                ex.Message;
                        }
                    }

                    TempData["SuccessMessage"] =
                        successCount +
                        " records uploaded successfully. " +
                        failCount +
                        " records failed.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Upload failed: " + ex.Message;

                return RedirectToAction("Index");
            }
        }

        // ================= HELPER METHOD =================

        private void AddDefaultColumn(
            SqlCommand cmd,
            List<string> matchedColumns,
            List<string> matchedParams,
            string columnName,
            string parameterName,
            object value)
        {
            if (!matchedColumns.Contains(columnName))
            {
                matchedColumns.Add(columnName);

                matchedParams.Add(parameterName);

                cmd.Parameters.AddWithValue(
                    parameterName,
                    value);
            }
        }
    }
}