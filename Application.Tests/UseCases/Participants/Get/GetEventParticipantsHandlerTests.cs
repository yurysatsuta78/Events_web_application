using Application.UseCases.Participants.Get;
using Application.UseCases.Participants.Get.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace Application.Tests.UseCases.Participants.Get
{
    public class GetEventParticipantsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetEventParticipantsHandler _handler;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public GetEventParticipantsHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetEventParticipantsHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        private List<Participant> CreateParticipantList()
        {
            return new List<Participant>
            {
                Participant.Create(Guid.NewGuid(), "Name", "Surname", DateTime.UtcNow, "Email", "12345678"),
                Participant.Create(Guid.NewGuid(), "Name", "Surname", DateTime.UtcNow, "Email", "12345678"),
            };
        }

        private List<GetParticipantResponce> CreateParticipantResponceList()
        {
            return new List<GetParticipantResponce>
            {
                new GetParticipantResponce { Id = Guid.NewGuid(), Name = "Name", Surname = "Surname",
                 BirthDay = DateTime.UtcNow, Email = "Email" },
                new GetParticipantResponce { Id = Guid.NewGuid(), Name = "Name", Surname = "Surname",
                 BirthDay = DateTime.UtcNow, Email = "Email" },
            };
        }

        private Event CreateTestEvent()
        {
            return Event.Create(Guid.NewGuid(), "Name", "Desc", DateTime.UtcNow, "Location", "Category", 5);
        }

        private Participant CreateTestParticipant()
        {
            return Participant.Create(Guid.NewGuid(), "Name", "SurName", DateTime.UtcNow, "Email", "12345678");
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedParticipants_WhenEventExistsWithParticipants()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var testEvent = CreateTestEvent();
            testEvent.AddParticipant(CreateTestParticipant());
            testEvent.AddParticipant(CreateTestParticipant());

            var mappedParticipants = CreateParticipantResponceList();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testEvent);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<GetParticipantResponce>>(testEvent.Participants))
                .Returns(mappedParticipants);

            // Act
            var result = await _handler.Handle(eventId, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedParticipants, result);
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper
                .Map<IEnumerable<GetParticipantResponce>>(testEvent.Participants), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(eventId, _cancellationToken)
            );

            Assert.Equal("Event not found.", exception.Message);
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper
                .Map<IEnumerable<GetParticipantResponce>>(It.IsAny<IEnumerable<Participant>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenEventHasNoParticipants()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            var testEvent = CreateTestEvent();
            testEvent.AddParticipant(CreateTestParticipant());
            testEvent.AddParticipant(CreateTestParticipant());

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testEvent);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<GetParticipantResponce>>(testEvent.Participants))
                .Returns(new List<GetParticipantResponce>());

            // Act
            var result = await _handler.Handle(eventId, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetByIdWithIncludesAsync(eventId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper
                .Map<IEnumerable<GetParticipantResponce>>(testEvent.Participants), Times.Once);
        }
    }
}
