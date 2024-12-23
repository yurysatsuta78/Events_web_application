using AutoMapper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public sealed class ImagesRepository : IImagesRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ImagesRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddImage(Image imageDomain, Guid eventId, CancellationToken cancellationToken)
        {
            var imageEntity = new ImageDb
            {
                ImagePath = imageDomain.ImagePath,
                EventId = eventId
            };

            await _dbContext.Images.AddAsync(imageEntity, cancellationToken);
        }

        public async Task RemoveImage(int id, CancellationToken cancellationToken)
        {
            var imageEntity = await _dbContext.Images.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (imageEntity == null) 
            {
                throw new EntityNotFoundException($"{nameof(imageEntity)} not found!");
            }

            _dbContext.Images.Remove(imageEntity);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
