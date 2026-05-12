using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class TipoDocumento
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Descripcion { get; set; } = string.Empty;

    public int? CountValid { get; set; }
}
