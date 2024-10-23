namespace AdobeESignWebAPI.Model
{
    public class PostLibraryDocumentRequestModel
    {
        public string name { get; set; }
        public string sharingMode { get; set; }
        public string state { get; set; }
        public List<string> templateTypes { get; set; }
        public List<FileInfosModel> fileInfos { get; set; }
    }
}
