namespace Test1
{
    public interface IAuditable
    {
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public DateTime? DeletedDateUtc { get; set; }

        public string CreatedUser { get; set; }
        public string? UpdatedUser { get; set; }
        public string? DeletedUser { get; set; }
        public bool IsDeleted { get; set; }
    }
}

