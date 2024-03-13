using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class Category
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Image { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(150)]
    public string? ShortDesc { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<News> News { get; set; } = new List<News>();

    [InverseProperty("Category")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
