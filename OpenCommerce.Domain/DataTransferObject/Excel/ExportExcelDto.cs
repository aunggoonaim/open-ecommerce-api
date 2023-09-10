namespace OpenCommerce.Domain.DataTransferObject.Excel
{
    public class ExportExcelDto<T>
    {
        public Dictionary<string, string> Properties { get; set; } = null!;
        public List<T> Data { get; set; } = null!;
        public string SheetName { get; set; } = null!;
        public string? SaveAsFileName { get; set; }
    }
}