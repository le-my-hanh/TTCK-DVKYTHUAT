using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class OrderDetail
{
    [Key]
    [Column("OrderDetailID")]
    public int OrderDetailId { get; set; }

    [Column("OrderID")]
    public int? OrderId { get; set; }

    [Column("ServiceID")]
    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }

    public double? Total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("OrderDetails")]
    public virtual Service? Service { get; set; }
}
