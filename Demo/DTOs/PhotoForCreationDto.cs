using System;
using Microsoft.AspNetCore.Http;

namespace Demo.DTOs
{
    public class PhotoForCreationDto
    {
        public string  Url { get; set; }
        public IFormFile File { get; set; }
        public string Desctiprion { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}