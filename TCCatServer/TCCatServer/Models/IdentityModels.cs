﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TCCatServer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public int Level { get; set; }

        public System.DateTime JoinDate { get; set; }

        public virtual ICollection<Forum> Forums { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<PostLike> PostLikes { get; set; }

        public virtual ICollection<ThreadLike> ThreadLikes { get; set; }

        public virtual ICollection<ThreadFavorite> ThreadFavorites { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ThreadFavorite> ThreadFavorites { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<ThreadLike> ThreadLikes { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;

            // Lazy loading目前默认为false, 将来为了提高性能，可以考虑改成true，这样当需要reference数据的时候，就要显示的include进来，如
            //List<ParentClass> parents = context.Parents            // .Include("Children")
            // .ToList();
            //return parents;
            //Configuration.LazyLoadingEnabled = true;            
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Forums)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Threads)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.ThreadLikes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.ThreadFavorites)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.PostLikes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Forum>()
                .HasMany(e => e.Threads)
                .WithRequired(e => e.Forum)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.PostLikes)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.ChildrenPosts)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Thread>()
                .HasMany(e => e.Favoriates)
                .WithRequired(e => e.Thread)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Thread>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Thread)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Thread>()
                .HasMany(e => e.ThreadLikes)
                .WithRequired(e => e.Thread)
                .WillCascadeOnDelete(false);
        }

    }
}