using Application.UseCases.Participants.Delete;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace Application.Tests.UseCases.Participants.Delete
{
    public class DeleteParticipantHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteParticipantHandler _handler;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public DeleteParticipantHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteParticipantHandler(_mockUnitOfWork.Object);
        }

        private Participant CreateTestParticipant() 
        {
            return Participant.Create(Guid.NewGuid(), "Name", "SurName", DateTime.UtcNow, "Email", "12345678");
        }


        [Fact]
        public async Task Handle_ShouldDeleteParticipant_WhenParticipantExists()
        {
            // Arrange
            var participantId = Guid.NewGuid();
            var testParticipant = CreateTestParticipant();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository
                .GetByIdAsync(participantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testParticipant);

            // Act
            await _handler.Handle(participantId, _cancellationToken);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.ParticipantsRepository.Delete(testParticipant), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenParticipantDoesNotExist()
        {
            // Arrange
            var participantId = Guid.NewGuid();

            _mockUnitOfWork
                .Setup(uow => uow.ParticipantsRepository.GetByIdAsync(participantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Participant?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(participantId, _cancellationToken)
            );

            Assert.Equal("Participant not found.", exception.Message);
            _mockUnitOfWork.Verify(uow => uow.ParticipantsRepository.Delete(It.IsAny<Participant>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
