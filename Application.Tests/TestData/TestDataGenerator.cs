using Application.UseCases.Participants.Get.DTOs;
using Domain.Models;

namespace Application.Tests.TestData
{
    public static class TestDataGenerator
    {
        public static Participant CreateTestParticipant()
        {
            return Participant.Create(Guid.NewGuid(), "Name", "SurName", DateTime.UtcNow, "Email", "12345678");
        }

        public static Event CreateTestEvent()
        {
            return Event.Create(Guid.NewGuid(), "Name", "Desc", DateTime.UtcNow, "Location", "Category", 5);
        }

        public static Event CreateTestEventWithParticipants()
        {
            var testEvent = CreateTestEvent();
            testEvent.AddParticipant(CreateTestParticipant());
            testEvent.AddParticipant(CreateTestParticipant());

            return testEvent;
        }

        public static List<Participant> CreateParticipantList()
        {
            return new List<Participant>
            {
                Participant.Create(Guid.NewGuid(), "Name", "Surname", DateTime.UtcNow, "Email", "12345678"),
                Participant.Create(Guid.NewGuid(), "Name", "Surname", DateTime.UtcNow, "Email", "12345678"),
            };
        }

        public static List<GetParticipantResponce> CreateParticipantResponceList()
        {
            return new List<GetParticipantResponce>
            {
                new GetParticipantResponce { Id = Guid.NewGuid(), Name = "Name", Surname = "Surname",
                 BirthDay = DateTime.UtcNow, Email = "Email" },
                new GetParticipantResponce { Id = Guid.NewGuid(), Name = "Name", Surname = "Surname",
                 BirthDay = DateTime.UtcNow, Email = "Email" },
            };
        }
    }
}
