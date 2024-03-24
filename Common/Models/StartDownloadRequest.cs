using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class StartDownloadRequest
    {
        [Required]
        public string ServiceId { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Quality { get; set; }

    }
}
