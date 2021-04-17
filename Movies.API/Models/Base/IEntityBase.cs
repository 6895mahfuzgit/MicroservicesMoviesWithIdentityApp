namespace Movies.API.Models.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
