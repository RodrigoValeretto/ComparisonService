using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace ComparisonService.Entities;

public class Comparison
{
    [System.ComponentModel.DataAnnotations.Key]
    public long id { get; set; }

    public double[] embeddings1 { get; set; } = null!;
    
    public double[] embeddings2 { get; set; } = null!;

    public bool equals { get; set; }
}
