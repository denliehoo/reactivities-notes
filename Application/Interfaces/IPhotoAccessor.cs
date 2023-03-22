using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    // these wont affect our DB. its only for uploading/delete to cloudinary
    public interface IPhotoAccessor
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}