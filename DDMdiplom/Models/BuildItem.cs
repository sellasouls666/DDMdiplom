using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDMdiplom.Models
{
    [Table("BuildItems")]
    public class BuildItem
    {
        [Key]
        public int Id { get; set; }

        public int BuildId { get; set; }
        public Build? Build { get; set; }

        [Required, MaxLength(100)]
        public string ComponentType { get; set; } = string.Empty;   // "Процессор", "Видеокарта", …

        public int ComponentId { get; set; }   // ID в соответствующей таблице каталога

        // Свойства позиции сборки (не из каталога)
        public int ModuleCount { get; set; } = 1;            // для памяти
        public string? StorageInterface { get; set; }       // "SATA" или "M.2" для накопителей (сохраняем для удобства)
    }
}