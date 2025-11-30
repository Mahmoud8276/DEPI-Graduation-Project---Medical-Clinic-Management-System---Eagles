using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.FormFiles
{
    public interface IFileService
    {
        public Task<string?> UploadFileAsync(IFormFile file);
        public Task<(bool Success, string Message)> DeleteImageByUrlAsync(string? imageUrl);
    }
}
