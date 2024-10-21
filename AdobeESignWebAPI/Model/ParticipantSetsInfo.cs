namespace AdobeESignWebAPI.Model
{
    public class ParticipantSetsInfo
    {
        public int order { get; set; }
        public string role { get; set; }
        public List<MemberInfos> memberInfos { get; set; }
    }
}
