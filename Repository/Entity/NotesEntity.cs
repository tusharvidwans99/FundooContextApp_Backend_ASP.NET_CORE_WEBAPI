using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class NotesEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long noteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }

        public string Color { get; set; }

        public string Image { get; set; }

        public bool Archive { get; set; }

        public bool Pin { get; set; }

        public bool Trash { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }


        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual UserEntity User { get; set; }

    }
}
