using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCatServer.Models
{
    public class PostLike
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public Guid PostId { get; set; }

        public bool IsLike { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}