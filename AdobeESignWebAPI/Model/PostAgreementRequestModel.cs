namespace AdobeESignWebAPI.Model
{
    public class PostAgreementRequestModel
    {
        public List<FileInfos> fileInfos { get; set; }
        public string name { get; set; }
        public List<ParticipantSetsInfo> participantSetsInfo { get; set; }

        public string signatureType { get; set; }
        public string state { get; set; }
    }
}
