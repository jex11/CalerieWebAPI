namespace AdobeESignWebAPI.Model
{
    public class FileUploadRequestModel
    {
        public IFormFile file { get; set; }
        public string email { get; set; }
    }
}
