namespace OpenCommerce.Domain.DataTransferObject.Cache
{
    public record AttempLoginTrans
    {
        public string email { get; set; } = null!;
        public string ipaddress { get; set; } = null!;
        public int count { get; set; }
    }
}