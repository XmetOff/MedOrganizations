using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOrg.Models
{
    public class Patient
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Отчество")]
        public string MiddleName { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("ИИН")]
        public string IIN { get; set; }
        public AttachRequest AttachRequest { get; set; }
        public MedOrganization MedOrganization { get; set; }

    }
}