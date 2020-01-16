using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Empower.Entities
{
    public abstract class AuditableEntity  
    {
        [ScaffoldColumn(false)]
        [IgnoreDataMember]
        public DateTime SystemCreatedDateTime { get; set; }

        [ScaffoldColumn(false)]
        [IgnoreDataMember]
        public DateTime UserCreatedDateTime { get; set; }   

        [ScaffoldColumn(false)]
        [IgnoreDataMember]
        public int CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [IgnoreDataMember]
        public DateTime? UpdatedDateTime { get; set; }  
        
        [ScaffoldColumn(false)]
        [IgnoreDataMember]
        public int? UpdatedBy { get; set; }

        public int Id { get; set; }

        [IgnoreDataMember]
        public bool DeleteFlag { get; set; }
    }
}
