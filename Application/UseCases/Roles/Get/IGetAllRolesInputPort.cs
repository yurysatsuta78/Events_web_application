using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.Roles.Get
{
    public interface IGetAllRolesInputPort : IInputPort
    {
        Task<IEnumerable<Role>> Handle(CancellationToken cancellationToken);
    }
}
