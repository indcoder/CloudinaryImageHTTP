public interface IImageUploadService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string cloudname, string apikey, string signature, string timestamp);
}
