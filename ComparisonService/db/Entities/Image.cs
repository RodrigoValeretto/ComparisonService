using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace ComparisonService.Entities;

public class Image
{
    [System.ComponentModel.DataAnnotations.Key]
    public Guid Guid { get; set; }

    public string Embeddings { get; set; } = null!;
}
