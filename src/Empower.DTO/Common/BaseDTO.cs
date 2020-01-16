using System;

namespace Empower.DTO
{   
    public abstract class BaseDTO  
    {
        public DateTime SystemCreatedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public DateTime UserCreatedDateTime { get; set; }

    }
}
