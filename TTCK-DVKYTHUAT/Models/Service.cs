using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class Service
{
    [Key]
    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [StringLength(150)]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Image { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Services")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<Conment> Conments { get; set; } = new List<Conment>();

    [InverseProperty("Service")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
