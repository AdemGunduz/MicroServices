﻿using Course.Service.PhotoStock.Dtos;
using Course.Shread.ControllerBases;
using Course.Shread.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course.Service.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {



        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                
                var returnPath = "photos/"+photo.FileName;

               PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Succes(photoDto, 200));

            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));

        }
        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoUrl);//combine birleştir

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));

            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Succes(204));
        }

    }
}
