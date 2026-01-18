namespace Identity.Entity.Commons
{
    public abstract class BaseEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
