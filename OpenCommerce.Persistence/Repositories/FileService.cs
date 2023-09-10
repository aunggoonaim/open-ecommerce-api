using SkiaSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenCommerce.Application.Repositories;
using Microsoft.AspNetCore.Hosting;
using OpenCommerce.Domain.Setting;
using OpenCommerce.Domain.DataTransferObject.Asset;

namespace OpenCommerce.Persistence.Repositories;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AppSetting appSettings;

    public FileService(IWebHostEnvironment webHostEnvironment, IOptions<AppSetting> optionSetting)
    {
        _webHostEnvironment = webHostEnvironment;
        appSettings = optionSetting.Value;
    }

    public async Task<string> SaveImagePhoto(IFormFile file)
    {
        var UrlImage = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload", "Photo");

        CreateDirectorryIfNotExist(UrlImage);

        var Reducer = await ReduceImage(file);

        if (Reducer.Data == null || Reducer.FileName == null)
        {
            throw new Exception("Reduce image has been invalid format.");
        }

        var FileUrl = Path.Combine(UrlImage, Reducer.FileName);

        await File.WriteAllBytesAsync(FileUrl, Reducer.Data);

        return Reducer.FileName;
    }

    public async Task<string> SaveImageDocument(IFormFile file)
    {
        var UrlDocument = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload", "Document");

        CreateDirectorryIfNotExist(UrlDocument);

        var BytesData = await BytesFromIFormFile(file);

        if (BytesData is null)
        {
            throw new Exception("Data is invalid format.");
        }

        var CurrentContentType = file.FileName.Split('.').LastOrDefault();
        string FileName = string.Concat(Guid.NewGuid().ToString(), CurrentContentType);

        var FileUrl = Path.Combine(UrlDocument, FileName);

        await File.WriteAllBytesAsync(FileUrl, BytesData);

        return FileName;
    }

    public async Task<byte[]> ReadImagePhoto(string FileName)
    {
        var UrlImage = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload", "Photo");

        CreateDirectorryIfNotExist(UrlImage);

        return await File.ReadAllBytesAsync(Path.Combine(UrlImage, FileName));
    }

    public async Task<byte[]> ReadImageDocument(string FileName)
    {
        var UrlImage = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload", "Document");

        CreateDirectorryIfNotExist(UrlImage);

        return await File.ReadAllBytesAsync(Path.Combine(UrlImage, FileName));
    }

    private void CreateDirectorryIfNotExist(string Url)
    {
        if (!Directory.Exists(Url))
        {
            Directory.CreateDirectory(Url);
        }
    }

    private async Task<ImageData> ReduceImage(IFormFile file)
    {
        var MaxSize = appSettings.DefaultImageWidth;
        
        var ContentTypes = new string[] { "jpg", "jpeg", "png", "bmp", "gif", "webp", "avif" };

        var CurrentContentType = file.FileName.Split('.').LastOrDefault();

        var CountContentType = ContentTypes.Where(x => x == CurrentContentType).FirstOrDefault() ?? throw new Exception("Not support this file type.");
        string FileName = string.Concat(Guid.NewGuid().ToString(), ".png");

        DateTime now = DateTime.Now;
        // var ThaiZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        // var ThaiTime = TimeZoneInfo.ConvertTimeFromUtc(now, ThaiZone);
        var bytesImage = await BytesFromIFormFile(file) ?? throw new Exception("File data has been null.");
        var (FileContents, Height, Width) = SkiaResize(bytesImage, MaxSize);
        return new ImageData() { Data = FileContents, FileName = FileName };
    }

    private async Task<byte[]> BytesFromIFormFile(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    private (byte[] FileContents, int Height, int Width) SkiaResize(byte[] fileContents, int MaxSize, SKFilterQuality quality = SKFilterQuality.Low)
    {
        using MemoryStream ms = new(fileContents);
        using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

        float ratioW = MaxSize;
        float ratioH = MaxSize;
        if (sourceBitmap.Height > MaxSize || sourceBitmap.Width > MaxSize)
        {
            if (sourceBitmap.Height > sourceBitmap.Width)
            {
                float r = (float)MaxSize / (float)sourceBitmap.Height;
                ratioH = MaxSize;
                ratioW = (float)sourceBitmap.Width * r;
            }
            if (sourceBitmap.Width > sourceBitmap.Height)
            {
                float r = (float)MaxSize / (float)sourceBitmap.Width;
                ratioW = MaxSize;
                ratioH = (float)sourceBitmap.Height * r;
            }
        }
        else
        {
            ratioW = sourceBitmap.Width;
            ratioH = sourceBitmap.Height;
        }

        int height = Math.Min((int)(ratioH), sourceBitmap.Height);
        int width = Math.Min((int)(ratioW), sourceBitmap.Width);

        using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), quality);
        using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
        using SKData data = scaledImage.Encode(SKEncodedImageFormat.Png, 75);

        return (data.ToArray(), height, width);
    }
}
