namespace Movies.Client.Models.Base
{
    public interface IEntityBase<TId>
    {
        TId Id { get; }
    }
}
