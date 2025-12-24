using Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace Pronia.Models
{
    public class Category : BaseEntity
    {
       
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
