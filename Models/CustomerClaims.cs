using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace TaskAuthenticationAuthorization.Models
{
    public static class CustomerClaims
    {
        public static Claim GetProperClaim(Discount? discountClaim)
        {
            if (discountClaim == null)
                return new Claim("buyerType", "regular");

            return discountClaim switch
            {
                Discount.None => new Claim("buyerType", "none"),
                Discount.Regular => new Claim("buyerType", "regular"),
                Discount.Golden => new Claim("buyerType", "golden"),
                Discount.Wholesale => new Claim("buyerType", "wholesale"),
                
            };
        }
       
    }
}
