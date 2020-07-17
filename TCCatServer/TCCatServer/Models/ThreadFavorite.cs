using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCatServer.Models
{
    public class ThreadFavorite
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public Guid ThreadId { get; set; }

        public virtual Thread Thread { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}