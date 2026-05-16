namespace Infrastructure.Storage
{
    public class R2Options
    {
        public string AccountId { get; set; } = "";
        public string AccessKeyId { get; set; } = "";
        public string SecretAccessKey { get; set; } = "";
        public string BucketName { get; set; } = "";
        public string PublicBaseUrl { get; set; } = "";
    }
}
