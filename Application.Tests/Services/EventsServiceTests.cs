using Application.Interfaces.UnitsOfWork;
using Application.Services;
using Domain.Exceptions;
using Domain.Models;
using Moq;

namespace Application.Tests.Services
{
    public class EventsServiceTests
    {
        private readonly Mock<IEventParticipantUOW> _mockUnitOfWork;
        private readonly EventsService _eventsService;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public EventsServiceTests() 
        {
            _mockUnitOfWork = new Mock<IEventParticipantUOW>();
            _eventsService = new EventsService(this._mockUnitOfWork.Object);
        }

        private Event CreateSampleEvent()
        {
            return Event.Create(Guid.NewGuid(), "Event", "Description",
                DateTime.UtcNow.AddDays(1), "Location", "Category", 20);
        }

        private Participant CreateSampleParticipant()
        {
            return Participant.Create(Guid.NewGuid(), "Name", "Surname",
                DateTime.Parse("10.10.2003"), "Email", "123123123123123");
        }

        [Fact]
        public async Task CreateEvent_ShouldCallAddEventAndSaveAsync() 
        {
            //Arrange
            var testEvent = CreateSampleEvent();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.AddEvent(testEvent, _cancellationToken));

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync(_cancellationToken));

            //Act
            await _eventsService.CreateEvent(testEvent, _cancellationToken);

            //Assert
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .AddEvent(testEvent, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Once);
        }



        [Fact]
        public async Task CreateEvent_ShouldThrowException_WhenAddEventFails() 
        {
            //Arrange
            var testEvent = CreateSampleEvent();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.AddEvent(testEvent, _cancellationToken));

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync(_cancellationToken))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _eventsService.CreateEvent(testEvent, _cancellationToken)
            );

            Assert.Equal("Database error", exception.Message);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .AddEvent(testEvent, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Once);
        }



        [Fact]
        public async Task AddParticipant_ShouldAddParticipantAndSaveAsync() 
        {
            //Arrange
            var testEvent = CreateSampleEvent();
            var testParticipant = CreateSampleParticipant();

            var eventId = testEvent.Id;
            var participantId = testParticipant.Id;

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository
                .GetParticipantById(participantId, _cancellationToken))
                .ReturnsAsync(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventById(eventId, _cancellationToken))
                .ReturnsAsync(testEvent);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .UpdateEventParticipants(testEvent, _cancellationToken));

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync(_cancellationToken));

            // Act
            await _eventsService.AddParticipant(eventId, participantId, _cancellationToken);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.ParticipantsRepository
                .GetParticipantById(participantId, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetEventById(eventId, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .UpdateEventParticipants(testEvent, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Once);
        }



        [Fact]
        public async Task AddParticipant_ShouldThrowException_WhenParticipantAlreadyRegistered()
        {
            // Arrange
            var testEvent = CreateSampleEvent();
            var testParticipant = CreateSampleParticipant();
            
            var eventId = testEvent.Id;
            var participantId = testParticipant.Id;

            testEvent.AddParticipant(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetParticipantById(participantId, _cancellationToken))
                .ReturnsAsync(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventById(eventId, _cancellationToken))
                .ReturnsAsync(testEvent);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _eventsService.AddParticipant(eventId, participantId, _cancellationToken)
            );

            Assert.Equal($"Participant {participantId} already enrolled.", exception.Message);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
            .UpdateEventParticipants(testEvent, _cancellationToken), Times.Never);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Never);
        }



        [Fact]
        public async Task AddParticipant_ShouldThrowException_WhenMaxParticipantsReached() 
        {
            //Arange
            var testEvent = Event.Create(Guid.NewGuid(), "Event", "Description",
                DateTime.UtcNow.AddDays(1), "Location", "Category", 1);
            var testParticipant = CreateSampleParticipant();

            var eventId = testEvent.Id;
            var participantId = testParticipant.Id;

            testEvent.AddParticipant(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetParticipantById(participantId, _cancellationToken))
                .ReturnsAsync(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventById(eventId, _cancellationToken))
                .ReturnsAsync(testEvent);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _eventsService.AddParticipant(eventId, participantId, _cancellationToken)
            );

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .UpdateEventParticipants(testEvent, _cancellationToken), Times.Never);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Never);
        }



        [Fact]
        public async Task AddParticipant_ShouldThrowException_WhenParticipantNotFound()
        {
            // Arrange
            var participantId = Guid.NewGuid();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository
                .GetParticipantById(It.IsAny<Guid>(), _cancellationToken))
                .ThrowsAsync(new EntityNotFoundException("Participant not found."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _eventsService.AddParticipant(It.IsAny<Guid>(), participantId, _cancellationToken)
            );

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetEventById(It.IsAny<Guid>(), _cancellationToken), Times.Never);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .UpdateEventParticipants(It.IsAny<Event>(), _cancellationToken), Times.Never);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Never);
        }



        [Fact]
        public async Task AddParticipant_ShouldThrowException_WhenEventNotFound()
        {
            // Arrange
            var testParticipant = CreateSampleParticipant();

            var eventId = Guid.NewGuid();
            var participantId = Guid.NewGuid();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository
                .GetParticipantById(participantId, _cancellationToken))
                .ReturnsAsync(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .GetEventById(eventId, _cancellationToken))
                .ThrowsAsync(new EntityNotFoundException("Event not found."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _eventsService.AddParticipant(eventId, participantId, _cancellationToken)
            );

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .UpdateEventParticipants(It.IsAny<Event>(), _cancellationToken), Times.Never);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Never);
        }



        [Fact]
        public async Task RemoveParticipant_ShouldRemoveParticipantAndSaveAsync()
        {
            // Arrange
            var testEvent = CreateSampleEvent();
            var testParticipant = CreateSampleParticipant();

            var eventId = testEvent.Id;
            var participantId = testParticipant.Id;

            testEvent.AddParticipant(testParticipant);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventById(eventId, _cancellationToken))
                .ReturnsAsync(testEvent);

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository
                .UpdateEventParticipants(testEvent, _cancellationToken));

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync(_cancellationToken));

            // Act
            await _eventsService.RemoveParticipant(eventId, participantId, _cancellationToken);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetEventById(eventId, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .UpdateEventParticipants(testEvent, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Once);
        }



        [Fact]
        public async Task GetEventById_ShouldReturnEvent()
        {
            // Arrange
            var testEvent = CreateSampleEvent();

            var eventId = testEvent.Id;

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventById(eventId, _cancellationToken))
                .ReturnsAsync(testEvent);

            // Act
            var result = await _eventsService.GetEventById(eventId, _cancellationToken);

            // Assert
            Assert.Equal(testEvent, result);
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetEventById(eventId, _cancellationToken), Times.Once);
        }



        [Fact]
        public async Task GetEventByName_ShouldReturnEvent()
        {
            // Arrange
            var testEvent = CreateSampleEvent();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.GetEventByName(It.IsAny<string>(), _cancellationToken))
                .ReturnsAsync(testEvent);

            // Act
            var result = await _eventsService.GetEventByName("AnyString", _cancellationToken);

            // Assert
            Assert.Equal(testEvent, result);
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .GetEventByName(It.IsAny<string>(), _cancellationToken), Times.Once);
        }



        [Fact]
        public async Task DeleteEvent_ShouldCallDeleteEventAndSaveAsync()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _mockUnitOfWork
                .Setup(uow => uow.EventsRepository.DeleteEvent(eventId, _cancellationToken));

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync(_cancellationToken));

            // Act
            await _eventsService.DeleteEvent(eventId, _cancellationToken);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.EventsRepository
                .DeleteEvent(eventId, _cancellationToken), Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(_cancellationToken), Times.Once);
        }
    }
}
