using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Coreapi.Common.Enums;

namespace Coreapi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string MacAddress { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Required]
        [DefaultValue(0)]
        public double Score { get; set; }

        [StringLength(500)]
        public string Avatar { get; set; }

        [StringLength(80)]
        public string ClientId { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [StringLength(250)]
        public string IntegrateId { get; set; }
        public CurrentUserTypeEnum Type { get; set; }

        public string NickName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int IdSection { get; set; }
        public int IdCity { get; set; }
        public string NaCode { get; set; }
        public bool IsAdminSupport { get; set; } = false;

        [StringLength(50)]
        public string BaleId { get; set; }

    }
}
