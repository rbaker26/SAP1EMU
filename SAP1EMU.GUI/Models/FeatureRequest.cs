using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class FeatureRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A title is necessary to request a feature.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A description of the feature is needed in order to decide if we want to implement it.")]
        [Display(Name = "Feature description")]
        public string Description { get; set; }

        public string SAP1EMUVersion { get; set; }
    }
}
