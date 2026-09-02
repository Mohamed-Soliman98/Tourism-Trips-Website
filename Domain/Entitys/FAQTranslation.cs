using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitys
{
    public class FAQTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public Guid FAQId { get; set; }
        public FAQ FAQ { get; set; } = null!;
    }
}
