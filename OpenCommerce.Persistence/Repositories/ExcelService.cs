using ClosedXML.Excel;
using System.Data;
using System.Reflection;
using ExcelDataReader;
using OpenCommerce.Application.Repositories;
using Microsoft.AspNetCore.Hosting;
using OpenCommerce.Domain.DataTransferObject.Excel;
using Microsoft.AspNetCore.Http;

namespace OpenCommerce.Persistence.Repositories;

public class ExcelService : IExcelService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ExcelService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task GenerateExcel<T>(ExportExcelDto<T> file, string[] location)
    {
        using var workbook = new XLWorkbook();
        if (file.Properties.Count == 0)
        {
            throw new Exception("FieldName and FieldTitleName is not define.");
        }

        var TableDt = ToDataTable<T>(file.Data, file.Properties);
        var worksheet = workbook.Worksheets.Add(TableDt, file.SheetName);

        if (file.SaveAsFileName == null)
        {
            throw new Exception("SaveAsFileName argument has been null.");
        }

        var FileLocationCombine = string.Concat(_webHostEnvironment.ContentRootPath, location, file.SaveAsFileName);

        await Task.Delay(1000);
        workbook.SaveAs(FileLocationCombine);
    }

    public DataTable? ConvertExcelStreamToDataTable(IFormFile file)
    {
        try
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var fs = file.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(fs);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                UseColumnDataType = true,
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            fs.Close();
            return result.Tables[0];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public async Task<byte[]> ExportExcel<T>(ExportExcelDto<T> f)
    {
        var workbookBytes = Array.Empty<byte>();
        using (var workbook = new XLWorkbook())
        {
            if (f.Properties.Count == 0)
            {
                throw new Exception("FieldName and FieldTitleName is not define.");
            }

            using var ms = new MemoryStream();
            var TableDt = ToDataTable<T>(f.Data, f.Properties);
            var worksheet = workbook.Worksheets.Add(TableDt, f.SheetName);
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(ms);
            workbookBytes = ms.ToArray();
            await ms.FlushAsync();
            ms.Close();
        }
        return await Task.FromResult(workbookBytes);
    }

    public async Task<byte[]> ExportExcelMultiSheet<T>(List<ExportExcelDto<T>> file)
    {
        var workbookBytes = Array.Empty<byte>();
        using (var workbook = new XLWorkbook())
        {
            foreach (var f in file)
            {
                if (f.Properties.Count == 0)
                {
                    throw new Exception("FieldName and FieldTitleName is not define.");
                }
            }

            foreach (var f in file)
            {
                using var ms = new MemoryStream();
                var TableDt = ToDataTable<T>(f.Data, f.Properties);
                var worksheet = workbook.Worksheets.Add(TableDt, f.SheetName);
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(ms);
                workbookBytes = ms.ToArray();
                await ms.FlushAsync();
                ms.Close();
            }
        }
        return await Task.FromResult(workbookBytes);
    }

    private DataTable ReadWorkSheetToDataTable(IXLWorksheet workSheet)
    {
        // Create a new DataTable.
        DataTable dt = new DataTable();
        // Loop through the Worksheet rows.
        bool firstRow = true;
        foreach (IXLRow row in workSheet.Rows())
        {
            // Use the first row to add columns to DataTable.
            if (firstRow)
            {
                foreach (IXLCell cell in row.Cells())
                {
                    dt.Columns.Add(cell.Value.ToString());
                }
                firstRow = false;
            }
            else
            {
                // Add rows to DataTable.
                dt.Rows.Add();
                int i = 0;

                try
                {
                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                    {
                        dt.Rows[^1][i] = cell.Value;
                        i++;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
        return dt;
    }

    private DataTable ToDataTable<T>(List<T> items, Dictionary<string, string> properties)
    {
        DataTable dataTable = new(typeof(T).Name);
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => properties.ContainsKey(p.Name)).ToArray();
        foreach (PropertyInfo prop in Props)
        {
            dataTable.Columns.Add(properties[prop.Name]);
        }
        foreach (T item in items)
        {
            object?[] values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
}