using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConoceTe.Models
{
    public class Tarjeta
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.CreditCard)]
        public string CN { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Password)]
        public string CV { get; set; }

        [Required]
        public int NM { get; set; }

        [Required]
        public int NY { get; set; }
        public double MT { get; set; }
    }
}