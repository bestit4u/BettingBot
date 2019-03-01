using Newtonsoft.Json;

namespace PinnacleWrapper.Data
{
    public class TeamTotalPointsType
    {
        [JsonProperty(PropertyName = "away")]
        public TeamTotalPoints Away { get; set; }

        [JsonProperty(PropertyName = "home")]
        public TeamTotalPoints Home { get; set; }
    }
}
