using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCatServer.Models
{
    public class Forum
    {
        public Forum()
        {
            Threads = new HashSet<Thread>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}