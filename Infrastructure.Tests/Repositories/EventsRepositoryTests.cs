using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories
{
    public class EventsRepositoryTests
    {
        private readonly EventsDbContext _dbContext;
        private readonly EventsRepository _repository;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public EventsRepositoryTests() 
        {
            var options = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new EventsDbContext(options);
            _repository = new EventsRepository(_dbContext);
        }

        private Event CreateSampleEvent(Guid id)
        {
            return Event.Create(id, "Event", "Description",
                DateTime.UtcNow.AddDays(1), "Location", "Category", 20);
        }


        [Fact]
        public async Task Add_ShouldAddEventToDatabase()
        {
            // Arrange
            var testEvent = CreateSampleEvent(Guid.NewGuid());

            // Act
            _repository.Add(testEvent);
            await _repository.SaveAsync(_cancellationToken);

            // Assert
            var eventInDb = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == testEvent.Id, _cancellationToken);

            Assert.NotNull(eventInDb);
            Assert.Equal(testEvent.Id, eventInDb.Id);
            Assert.Equal(testEvent.Name, eventInDb.Name);
            Assert.Equal(testEvent.Description, eventInDb.Description);
            Assert.Equal(testEvent.EventTime, eventInDb.EventTime);
            Assert.Equal(testEvent.Location, eventInDb.Location);
            Assert.Equal(testEvent.Category, eventInDb.Category);
            Assert.Equal(testEvent.MaxParticipants, eventInDb.MaxParticipants);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent() 
        {
            //Arrange
            var testEvent = CreateSampleEvent(Guid.NewGuid());

            _dbContext.Events.Add(testEvent);
            await _dbContext.SaveChangesAsync();

            //Act
            var eventFromDb = await _repository.GetByIdAsync(testEvent.Id, _cancellationToken);

            // Assert
            Assert.NotNull(eventFromDb);
            Assert.Equal(eventFromDb.Id, testEvent.Id);
            Assert.Equal(eventFromDb.Name, testEvent.Name);
            Assert.Equal(eventFromDb.Description, testEvent.Description);
            Assert.Equal(eventFromDb.EventTime, testEvent.EventTime);
            Assert.Equal(eventFromDb.Location, testEvent.Location);
            Assert.Equal(eventFromDb.Category, testEvent.Category);
            Assert.Equal(eventFromDb.MaxParticipants, testEvent.MaxParticipants);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEntityNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId, _cancellationToken);

            // & Assert
            Assert.Null(result);
        }
    }
}
