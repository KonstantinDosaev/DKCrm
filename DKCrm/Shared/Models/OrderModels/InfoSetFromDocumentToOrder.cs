using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.OrderModels
{
    public class InfoSetFromDocumentToOrder
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public int DocumentType { get; set; }
        public int FileType { get; set; }
        public DateTime DateTimeCreated { get; set; }
        [MaxLength(200)]
        public string PathToFile { get; set; } = null!;
        public Guid OrderId { get; set; }
        public float StampPosition { get; set;}
    }
}
