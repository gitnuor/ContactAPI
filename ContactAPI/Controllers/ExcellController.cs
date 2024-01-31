using Dapper;
using goAMLNew24.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;

namespace ContactAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExcellController : ControllerBase
	{
		private readonly string _connectionString; // Replace with your SQL Server connection string

		public ExcellController()
		{
			// Replace with your SQL Server connection string
			_connectionString = "YourConnectionStringHere";
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
		{
			try
			{
				if (file == null || file.Length == 0)
					return BadRequest("Invalid file.");

				using (var stream = new MemoryStream())
				{
					await file.CopyToAsync(stream);
					using (var package = new ExcelPackage(stream))
					{
						var worksheet = package.Workbook.Worksheets[0];

						// Validate headers (assuming headers are in the first row)
						var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
							.Select(cell => cell.Text.Trim())
							.ToList();

						// Validate headers based on your requirements
						// For example, check if headers contain expected column names

						// Process data starting from the second row
						var dataRows = worksheet.Cells[2, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
						var data = new List<ExcelEntity>(); // Replace YourModel with your data model

						foreach (var row in dataRows)
						{
							var rowData = new ExcelEntity // Replace YourModel with your data model
							{
								// Map columns to properties of your model
								// For example:
								// Property1 = row[0].Text.Trim(),
								// Property2 = row[1].Text.Trim(),
								// ...
							};

							data.Add(rowData);
						}

						// Store data in the database using Dapper
						using (var connection = new SqlConnection(_connectionString))
						{
							connection.Open();

							// Assuming you have a table named YourTable in the database
							// Replace YourTable with your actual table name
							var insertQuery = "INSERT INTO YourTable (Column1, Column2, ...) VALUES (@Property1, @Property2, ...)";
							connection.Execute(insertQuery, data);
						}

						return Ok("File uploaded and data stored successfully.");
					}
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}

	}
}
