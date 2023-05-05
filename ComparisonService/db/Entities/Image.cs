using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace ComparisonService.Entities;

public class Image
{
    [System.ComponentModel.DataAnnotations.Key]
    public Guid guid { get; set; }

    public double[] embeddings { get; set; } = null!;
}
