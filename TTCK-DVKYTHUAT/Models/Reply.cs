using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTCK_DVKYTHUAT.Models;

public partial class Reply
{
    [Key]
    [Column("ReplyID")]
    public int ReplyId { get; set; }

    [Column("Reply")]
    [StringLength(150)]
    public string? Reply1 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RepDate { get; set; }

    [Column("ConmentID")]
    public int? ConmentId { get; set; }

    [ForeignKey("ConmentId")]
    [InverseProperty("Replies")]
    public virtual Conment? Conment { get; set; }
}
