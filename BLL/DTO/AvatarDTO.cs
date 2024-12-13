using DAL.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class AvatarDTO 
    {
        public IFormFile? ImageFile { get; set; } = null;
        public bool? IsDeletedImg { get; set; } = false;
        public string? Base64Image { get; set; } = null;
        public string? ImgName { get; set; } = null;

    }


}