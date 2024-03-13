using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class News
{
    [Key]
    [Column("NewsID")]
    public int NewsId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Image { get; set; }

    public string? Desciption { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("News")]
    public virtual Category? Category { get; set; }
}
