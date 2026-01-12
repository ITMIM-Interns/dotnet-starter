namespace MiniApp.API.ResponseModel
{
    public sealed class ExceptionResponse
    {
        public string  Message { get; set; }
        public int StatusCode { get; set; } = 500;
    }
}
