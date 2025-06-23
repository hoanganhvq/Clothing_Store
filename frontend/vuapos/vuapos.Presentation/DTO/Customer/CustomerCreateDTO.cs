using System;

namespace vuapos.Presentation.DTO.Customer
{
    public class CustomerCreateDTO
    {
        public required String name { get; set; }
        public required String phone { get; set; }
        public required String email { get; set; }

    }
}
