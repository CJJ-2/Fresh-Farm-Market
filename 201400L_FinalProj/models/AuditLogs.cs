using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _201400L_FinalProj.models
{
    public class AuditLogs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string Action { get; set; }
        public DateTime TimeStamp { get; set; }
       
    }
}
