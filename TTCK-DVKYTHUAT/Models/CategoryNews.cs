using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class CategoryNews
{
    [Key]
    [Column("CategorynewID")]
    public int CategorynewId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Image { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(150)]
    public string? ShortDesc { get; set; }

    [InverseProperty("Categorynew")]
    public virtual ICollection<News> News { get; set; } = new List<News>();
}
