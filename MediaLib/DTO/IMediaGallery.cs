using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLib.DTO
{
    public interface IMediaGallery
    {
        public List<MediaDatum> MediaData { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
}
