using Microsoft.AspNetCore.Http;

namespace OpenCommerce.Application.Repositories;

public interface IFileService
{
    Task<string> SaveImagePhoto(IFormFile file);

    Task<string> SaveImageDocument(IFormFile file);

    Task<byte[]> ReadImagePhoto(string FileName);

    Task<byte[]> ReadImageDocument(string FileName);
}
