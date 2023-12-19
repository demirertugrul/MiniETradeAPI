using ETradeAPI.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETradeAPI.Domain.Entities
{
    public class File : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }
        [NotMapped] // bu prop'u virtual olarak tanimlayip db'ye migrate edilmemesi icin.
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
    }
}
