using System.Data;
using Microsoft.AspNetCore.Http;
using OpenCommerce.Domain.DataTransferObject.Excel;

namespace OpenCommerce.Application.Repositories;

public interface IExcelService
{
    Task GenerateExcel<T>(ExportExcelDto<T> file, string[] location);
    DataTable? ConvertExcelStreamToDataTable(IFormFile file);
    Task<byte[]> ExportExcel<T>(ExportExcelDto<T> file);
    Task<byte[]> ExportExcelMultiSheet<T>(List<ExportExcelDto<T>> file);
}
