using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TCCatServer.Models
{
    public class Post
    {
        public Post()
        {
            PostLikes = new HashSet<PostLike>();
            ChildrenPosts = new HashSet<Post>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public Guid ThreadId { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime PostedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLike> PostLikes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> ChildrenPosts { get; set; }

        public virtual Post Parent { get; set; }

        public virtual Thread Thread { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}