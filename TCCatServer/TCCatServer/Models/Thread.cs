using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCatServer.Models
{
    public class Thread
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Thread()
        {
            Favoriates = new HashSet<ThreadFavorite>();
            Posts = new HashSet<Post>();
            ThreadLikes = new HashSet<ThreadLike>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public Guid ForumId { get; set; }

        public Guid UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThreadFavorite> Favoriates { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThreadLike> ThreadLikes { get; set; }
    }
}