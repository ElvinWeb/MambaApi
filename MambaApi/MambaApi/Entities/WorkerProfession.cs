namespace MambaApi.Entities
{
    public class WorkerProfession : BaseEntity  
    {
        public int WorkerId { get; set; }
        public int ProfessionId { get; set; }
        public Worker Worker { get; set; }
        public Profession Profession { get; set; }
    }
}
