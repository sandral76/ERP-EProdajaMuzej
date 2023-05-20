namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponses
    {
        public ApiValidationErrorResponse() : base(400)
        {
            
        }
        public IEnumerable<string> Errors { get; set; }
    }
}