namespace API.Dtos
{
    public class S3ObjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string PresignedUrl { get; set; } = string.Empty;
    }
}
