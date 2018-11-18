using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedOrg.Models
{
    public class AttachRequest
    {
        [Key]
        [ForeignKey("Patient")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }
        [DisplayName("Дата обработки")]
        public DateTime? HandleDate { get; set; }
        [Required]
        [DisplayName("Статус")]
        public string Status { get; set; }
        public Patient Patient { get; set; }
        public MedOrganization MedOrganization { get; set; }
    }
}