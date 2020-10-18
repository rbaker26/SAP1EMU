using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class BugReport
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "A title is necessary to report the bug.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A description of the bug is necessary in order to solve it.")]
        [Display(Name = "Bug description")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the steps in order to reproduce the bug in order for us to recreate it.")]
        [Display(Name = "Steps to reproduce the bug")]
        public string ReproductionSteps { get; set; }

        public string BrowserInfo { get; set; }

        public string SAP1EMUVersion { get; set; }
    }
}
