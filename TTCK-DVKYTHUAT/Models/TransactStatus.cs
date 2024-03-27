using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

[Table("TransactStatus")]
public partial class TransactStatus
{
    [Key]
    [Column("TransactStatusID")]
    public int TransactStatusId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public string? Description { get; set; }

    [InverseProperty("TransactStatus")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
