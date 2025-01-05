namespace Domain.Interfaces
{
    public interface IModel<TId>
    {
        TId Id { get; }
    }
}
