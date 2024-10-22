namespace AdobeESignWebAPI.Model
{
    public class PostAgreementRequestModel
    {
        public List<FileInfosModel> fileInfos { get; set; }
        public string name { get; set; }
        public List<ParticipantSetsInfoModel> participantSetsInfo { get; set; }

        public string signatureType { get; set; }
        public string state { get; set; }
    }
}
