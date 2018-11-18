using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOrg.Models
{
    public class MedOrganization
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}