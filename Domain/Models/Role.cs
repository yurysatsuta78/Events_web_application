namespace Domain.Models
{
    public class Role
    {
        public int Id { get; }
        public string Name { get; } = String.Empty;


        private Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private Role(string name)
        {
            Name = name;
        }


        public static Role GetDefaultRole()
        {
            return new Role(2, "Participant");
        }
    }
}
