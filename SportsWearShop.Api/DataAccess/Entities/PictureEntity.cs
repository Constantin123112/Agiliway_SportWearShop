namespace SportsWearShop.Api.DataAccess.Entities
{
    public class PictureEntity : BaseEntity
    {
        public string Filename { get; set; }
        public long? ProductId { get; set; }
        public ProductEntity Product { get; set; }

        public PictureEntity(long id)
        {
            Id = id;
        }

        public PictureEntity()
        {

        }
    }
}