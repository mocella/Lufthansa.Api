using System.ComponentModel.DataAnnotations;

namespace Lufthansa.Api.Models.Offers;

public enum CabinClass
{
    [Display(Name = "Economy")] Economy = 'M',

    [Display(Name = "Premium Economy")] PremiumEconomy = 'E',

    [Display(Name = "Business")] Business = 'C',

    [Display(Name = "First")] First = 'F'
}