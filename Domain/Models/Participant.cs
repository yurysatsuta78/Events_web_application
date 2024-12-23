namespace Domain.Models
{
    public class Participant
    {
        private readonly List<Role> _roles = new();

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public DateTime BirthDay { get; }
        public string Email { get; } = string.Empty;
        public string PasswordHash { get; } = string.Empty;
        public IReadOnlyCollection<Role> Roles => _roles;


        private Participant(Guid id, string name, string surname, DateTime birthDay, 
            string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Surname = surname;
            BirthDay = birthDay;
            Email = email;
            PasswordHash = passwordHash;
        }


        public static Participant Create(Guid id, string name, string surname, DateTime birthDay, 
            string email, string passwordHash) 
        {
            var participantDomain = new Participant(id, name, surname, birthDay, email, passwordHash);

            return participantDomain; 
        }


        public void AddRole(Role roleDomain) 
        {
            if (Roles.Any(r => r.Id == roleDomain.Id))
            {
                throw new InvalidOperationException($"Participant already has the role with Id: {roleDomain.Id}.");
            }

            _roles.Add(roleDomain);
        }
    }
}
