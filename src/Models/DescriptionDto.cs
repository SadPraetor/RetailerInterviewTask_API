using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class DescriptionDto {
        [Required]
        public string Description { get; set; }
    }
}
