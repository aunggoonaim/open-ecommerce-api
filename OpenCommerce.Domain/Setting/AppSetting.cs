namespace OpenCommerce.Domain.Setting;

public class AppSetting
{
    public string Secret { get; set; } = null!;
    public int DefaultImageWidth { get; set; }
    public bool Maintenance { get; set; }
    public string AesKey { get; set; } = null!;
    public string AesIV { get; set; } = null!;
}