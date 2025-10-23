using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Filminurk.ApplicationServices.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly FilminurkTARpe24Context _context;

        public FilesServices (IHostEnvironment webHost, FilminurkTARpe24Context context)
        {
            _webHost = webHost;
            _context = context;
        }

        public void FilesToApi(MoviesDTO dto, Movie domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "\\wwwroot\\multipleFileUpload\\");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        FileToApi path = new FileToApi
                        {
                            ImageID = Guid.NewGuid(),
                            ExistingFilePath = filePath,
                            MovieID = domain.ID,
                        };

                        _context.FilesToApi.AddAsync(path);
                    }

                } 
            }
        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            var imageId = await _context.FilesToApi.FirstOrDefaultAsync(x => x.ImageID == dto.ImageID);

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" + imageId.ExistingFilePath;
            
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FilesToApi.Remove(imageId);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                RemoveImageFromApi(dto);
            }

            return null;
        }
    }
}
