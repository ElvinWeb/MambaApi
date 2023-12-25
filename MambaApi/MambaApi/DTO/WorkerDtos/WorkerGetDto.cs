using FluentValidation;

namespace MambaApi.DTO.WorkerDtos
{
    public class WorkerGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        public string ImgUrl { get; set; }

    }
   
}
