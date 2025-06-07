using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using MeslekOdalari.Entity.Entities.Enums;


namespace MeslekOdalari.Entity.Entities
{
    public class AppUser:IdentityUser<ObjectId>
    {
        public AppUser()
        {
            Id = ObjectId.GenerateNewId();
        }

        public string NameSurName { get; set; }
        public string TC { get; set; }
        public UserRoles UserRole { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsApproved { get; set; }

        public string? CopyOfIDCardPath { get; set; }
        public string? ResidenceCertificatePath { get; set; }
        public string? TaxPlatePath { get; set; }
        public string? BusinessOpeningLicensePath { get; set; }
        public string? RegistrationFormPath { get; set; }
        public string? DiplomaOrCertificationPath { get; set; }
        public string? TradesmanRegistryDeclarationPath { get; set; }
        public string? SignatureDeclarationPath { get; set; }

    }
}
