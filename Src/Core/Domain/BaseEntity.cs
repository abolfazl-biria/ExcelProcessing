namespace Domain;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;

    public DateTime InsertTime { get; set; } = DateTime.Now;
}

public abstract class BaseEntity : BaseEntity<int>
{

}