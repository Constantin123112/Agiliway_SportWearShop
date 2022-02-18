namespace SportsWearShop.Api.Domain.Identity.Models
{
    public class FileDto
    {
        public long Id { get; set; }
        public byte[] Data { get; set; }
        public string Filename { get; set; }

        public FileDto()
        {

        }

        public FileDto(byte[] data, string filename)
        {
            Data = data;
            Filename = filename;
        }

        public FileDto(byte[] data, string filename, long Id)
        {
            this.Id = Id;
            Data = data;
            Filename = filename;
        }
    }
}