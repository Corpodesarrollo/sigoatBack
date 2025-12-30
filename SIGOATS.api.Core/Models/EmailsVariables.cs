using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGOATS.api.Core.Models
{
    public class EmailsVariables
    {
        [Key]
        public long Id { get; set; }
        public long EmailId { get; set; }

        [ForeignKey(nameof(EmailId))]
        public Emails? Emails { get; set; }
        public long VariableId { get; set; }

        [ForeignKey(nameof(VariableId))]
        public Variables? Variables { get; set; }
    }
}
