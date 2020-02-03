using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SulsApp.Models
{
    public class Submission
    {
        public Submission()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        [Required]
        [MaxLength(800)]
        public string Code { get; set; }
        public int AchievedResult { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProblemID { get; set; }
        public virtual Problem Problem { get; set; }
        public string UserId { get; set; }
        public virtual User Users { get; set; }
    }
}
