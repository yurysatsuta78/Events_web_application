using Application.DTO.Response.Event;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    public class EventsRepositoryTests
    {
        private readonly EventsDbContext _dbContext;
        private readonly EventsRepository _repository;
        private readonly IMapper _mapper;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public EventsRepositoryTests() 
        {
            var options = new DbContextOptionsBuilder<EventsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new EventsDbContext(options);


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Event, EventDb>();
                cfg.CreateMap<EventDb, Event>();
            });
            _mapper = config.CreateMapper();

            _repository = new EventsRepository(_dbContext, _mapper);
        }

        private Event CreateSampleEvent(Guid id)
        {
            return Event.Create(id, "Event", "Description",
                DateTime.UtcNow.AddDays(1), "Location", "Category", 20);
        }


        [Fact]
        public async Task AddEvent_ShouldAddEventToDatabase()
        {
            // Arrange
            var testEvent = CreateSampleEvent(Guid.NewGuid());

            // Act
            await _repository.AddEvent(testEvent, _cancellationToken);
            await _repository.SaveAsync(_cancellationToken);

            // Assert
            var eventInDb = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == testEvent.Id, _cancellationToken);

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
        public async Task GetEventById_ShouldReturnEvent() 
        {
            //Arrange
            var testEventEntity = _mapper.Map<EventDb>(CreateSampleEvent(Guid.NewGuid()));

            _dbContext.Events.Add(testEventEntity);
            await _dbContext.SaveChangesAsync();

            //Act
            var testEvent = await _repository.GetEventById(testEventEntity.Id, _cancellationToken);

            // Assert
            Assert.NotNull(testEvent);
            Assert.Equal(testEventEntity.Id, testEvent.Id);
            Assert.Equal(testEventEntity.Name, testEvent.Name);
            Assert.Equal(testEventEntity.Description, testEvent.Description);
            Assert.Equal(testEventEntity.EventTime, testEvent.EventTime);
            Assert.Equal(testEventEntity.Location, testEvent.Location);
            Assert.Equal(testEventEntity.Category, testEvent.Category);
            Assert.Equal(testEventEntity.MaxParticipants, testEvent.MaxParticipants);
        }


        [Fact]
        public async Task GetEventById_ShouldThrowEntityNotFoundException_WhenEventNotFound()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _repository.GetEventById(eventId, _cancellationToken));
        }
    }
}
