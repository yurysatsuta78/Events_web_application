using Application.Tests.TestData;
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


        [Fact]
        public async Task Handle_ShouldReturnMappedParticipants_WhenParticipantsExist()
        {
            // Arrange
            var participants = TestDataGenerator.CreateParticipantList();
            var mappedParticipants = TestDataGenerator.CreateParticipantResponceList();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(participants);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<GetParticipantResponce>>(participants))
                .Returns(mappedParticipants);

            // Act
            var result = await _handler.Handle(_cancellationToken);

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
            var result = await _handler.Handle(_cancellationToken);

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
