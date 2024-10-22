namespace AdobeESignWebAPI.Model
{
    public class ParticipantSetsInfoModel
    {
        public int order { get; set; }
        public string role { get; set; }
        public List<MemberInfosModel> memberInfos { get; set; }
    }
}
