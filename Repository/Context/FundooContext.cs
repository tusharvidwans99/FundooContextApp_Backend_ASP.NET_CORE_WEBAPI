using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {

        public FundooContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<UserEntity> UserTable { get; set; }

        public DbSet<NotesEntity> NotesTable { get; set; }

        public DbSet<CollaboratorEntity> CollaboratorTable { get; set; }

        public DbSet<LableEntity> lableTable { get; set; }

    }
}
