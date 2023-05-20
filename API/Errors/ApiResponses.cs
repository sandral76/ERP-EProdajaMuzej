namespace API.Errors
{
    public class ApiResponses
    {
        public ApiResponses(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Zahtev sadrzi neispravnu sintaksu ili nedostajuce parametare.",
                401 => "Zahtev za pristup resursu je odbijen.",
                403 => "Nemate prava pristupa resursu.",
                404 => "Traženi resurs nije pronadjen.",
                500 => "Neočekivana greska.",
                
            };
        }

    }
}