using System.Text.Json;

namespace DKCrm.Client.Constants
{
    public static class JsonOptions
    {
        public static readonly JsonSerializerOptions JsonIgnore = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            PropertyNamingPolicy = null
        };
    }
}
