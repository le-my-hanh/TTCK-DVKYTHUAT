using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

[Table("Conment")]
public partial class Conment
{
    [Key]
    [Column("ConmentID")]
    public int ConmentId { get; set; }

    [Column("ServiceID")]
    public int? ServiceId { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [StringLength(150)]
    public string? Message { get; set; }

    public double? Rating { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Conments")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Conment")]
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    [ForeignKey("ServiceId")]
    [InverseProperty("Conments")]
    public virtual Service? Service { get; set; }
}
