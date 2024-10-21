namespace AdobeESignWebAPI.Model
{
    public class AgreementViewModel
    {
        public string url { get; set; }
        public string name { get; set; }
        public string embeddedCode { get; set; }
        public DateTime expiration { get; set; }
        public bool isCurrent { get; set; }
    }
}
