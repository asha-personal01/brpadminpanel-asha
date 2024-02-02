//Developed by Nayan

namespace D1WebApp.Utilities.ApiResponse
{
    public class HttpPostResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }

        public HttpPostResponse CreateResponse(string status, string message, string redirecturl)
        {
            HttpPostResponse httppostresponse = new HttpPostResponse();
            httppostresponse.Status = status;
            httppostresponse.Message = message;
            httppostresponse.RedirectUrl = redirecturl;
            return httppostresponse;
        }
    }
}