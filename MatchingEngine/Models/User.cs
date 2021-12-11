namespace MatchingEngine.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public double Balance { get; set; } = 0;
    }
}
