using System.ComponentModel.DataAnnotations.Schema;

namespace MambaApi.Entities
{
    public class Worker : BaseEntity
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        public List<WorkerProfession> WorkerProfessions { get; set; }
        [NotMapped]
        public IFormFile ImgFile { get; set; }
        public string? ImgUrl { get; set; }
    }
}
