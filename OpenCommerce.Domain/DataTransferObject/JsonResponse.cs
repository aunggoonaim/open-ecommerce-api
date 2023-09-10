namespace OpenCommerce.Domain.DataTransferObject
{
    public class JsonResponse<T>
    {
        public bool IsError { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}