using Application.UseCases.Participants.Get;
using Application.UseCases.Participants.Get.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace Application.Tests.UseCases.Participants.Get
{
    public class GetAllParticipantsHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllParticipantsHandler _handler;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public GetAllParticipantsHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllParticipantsHandler(_mockUnitOfWork.Object, _mockMapper.Object);
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

        [Fact]
        public async Task Handle_ShouldReturnMappedParticipants_WhenParticipantsExist()
        {
            // Arrange
            var participants = CreateParticipantList();
            var mappedParticipants = CreateParticipantResponceList();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(participants);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<GetParticipantResponce>>(participants))
                .Returns(mappedParticipants);

            // Act
            var result = await _handler.Handle(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedParticipants, result);
            _mockUnitOfWork.Verify(uow => uow.ParticipantsRepository
                .GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper
                .Map<IEnumerable<GetParticipantResponce>>(participants), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoParticipantsExist()
        {
            // Arrange
            var participants = new List<Participant>();
            var mappedParticipants = new List<GetParticipantResponce>();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(participants);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<GetParticipantResponce>>(participants))
                .Returns(mappedParticipants);

            // Act
            var result = await _handler.Handle(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUnitOfWork.Verify(uow => uow.ParticipantsRepository
                .GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper
                .Map<IEnumerable<GetParticipantResponce>>(participants), Times.Once);
        }
    }
}
