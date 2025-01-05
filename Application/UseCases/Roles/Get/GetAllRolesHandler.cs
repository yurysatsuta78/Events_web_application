
using Application.UseCases.Events.Get.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.Roles.Get
{
    public class GetAllRolesHandler : IGetAllRolesInputPort
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllRolesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Role>> Handle(CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.RolesRepository.GetAllAsync(cancellationToken);

            return roles;
        }
    }
}
