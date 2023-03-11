namespace Pinchecito.Core.Interfaces
{
    public class Result<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public List<Error> Errors { get; set; } = new();
    }
}