using Domain.Interfaces;

namespace Domain.Models
{
    public class Role : IModel<int>
    {
        public int Id { get; }
        public string Name { get; } = String.Empty;


        private Role() { }


        private Role(int id, string name)
        {
            Id = id;
            Name = name;
        }


        public static Role Create(int id, string name) 
        {
            return new Role(id, name);
        }


        public static Role GetDefaultRole()
        {
            return new Role(2, "Participant");
        }
    }
}
