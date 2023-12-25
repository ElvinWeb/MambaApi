namespace MambaApi.Entities
{
    public class Profession : BaseEntity
    {
        public string Name { get; set; }
        public List<WorkerProfession> WorkerProfessions { get; set; }
    }
}
