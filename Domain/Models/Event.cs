using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Models
{
    public class Event : IModel<Guid>
    {
        private readonly List<Participant> _participants = new();
        private readonly List<Image> _images = new();

        public Guid Id { get; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime EventTime { get; private set; }
        public string Location { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public int MaxParticipants { get; private set; }
        public IReadOnlyCollection<Participant> Participants => _participants;
        public IReadOnlyCollection<Image> Images => _images;


        private Event() { }


        private Event(Guid id, string name, string description, DateTime eventTime, string location, string category,
            int maxParticipants)
        {
            Id = id;
            Name = name;
            Description = description;
            EventTime = eventTime;
            Location = location;
            Category = category;
            MaxParticipants = maxParticipants;
        }


        private Event(string name, string description, DateTime eventTime, string location, string category,
            int maxParticipants)
        {
            Name = name;
            Description = description;
            EventTime = eventTime;
            Location = location;
            Category = category;
            MaxParticipants = maxParticipants;
        }


        public static Event Create(Guid id, string name, string description, DateTime eventTime, string location, string category,
            int maxParticipants)
        {
            var eventDomain = new Event(id, name, description, eventTime, location, category, maxParticipants);

            return eventDomain;
        }


        public void UpdateEvent(string? name, string? description, DateTime? eventTime, string? location,
            string? category, int? maxParticipants)
        {
            var updatedEvent = new Event(
                name ?? Name,
                description ?? Description,
                eventTime ?? EventTime,
                location ?? Location,
                category ?? Category,
                maxParticipants ?? MaxParticipants
            );

            Name = updatedEvent.Name;
            Description = updatedEvent.Description;
            EventTime = updatedEvent.EventTime;
            Location = updatedEvent.Location;
            Category = updatedEvent.Category;
            MaxParticipants = updatedEvent.MaxParticipants;

            _participants.Clear();
            _images.Clear();
        }


        public void AddParticipant(Participant participant)
        {
            if (_participants.Count >= MaxParticipants)
            {
                throw new BadRequestException($"Maximum participants reached.");
            }

            if (_participants.Any(p => p.Id == participant.Id))
            {
                throw new BadRequestException($"Participant {participant.Id} already enrolled.");
            }

            _participants.Add(participant);
        }


        public void RemoveParticipant(Guid participantId)
        {
            var participant = _participants.Find(p => p.Id == participantId);
            if (participant == null)
            {
                throw new BadRequestException($"Participant with Id {participantId} not enrolled.");
            }

            _participants.Remove(participant);
        }


        public void AddImages(List<Image> images) 
        {
            _images.AddRange(images);
        }
    }
}
