using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI.CustomValidation
{
    public class UserModelRoleValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (UserModel)validationContext.ObjectInstance;

            if (user.Role == null)
                return new ValidationResult("Role is required.");

            var role = user.Role;


            return (role == "Admin" || role == "User")

                ? ValidationResult.Success
                : new ValidationResult($"Role '{user.Role}' is not supported");
        }
    }
}
