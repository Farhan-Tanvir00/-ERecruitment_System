using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ERecruitmentSystem02.Models.Domain
{
    public class UsersDetails
    {
        public int Id { get; set; }
        //Present Address...

        [DisplayName("Area/Village/House/Road")]
        [Required]
        public string PresentAreaOrVillageOrHouseOrRoad { get; set; }

        [DisplayName("Post Office")]
        [Required]
        public string PresentPostOffice { get; set; }

        [DisplayName("Police Station")]
        [Required]
        public string PresentPoliceStation { get; set; }

        [DisplayName("Postal Code")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "must contain only digits.")]
        public int PresentPostalCode { get; set; }

        [DisplayName("District")]
        [Required]
        public string PresentDistrict { get; set; }


        //Permanent Address...

        [DisplayName("Area/Village/House/Road")]
        [Required]
        public string PermanentAreaOrVillageOrHouseOrRoad { get; set; }

        [DisplayName("Post Office")]
        [Required]
        public string PermanentPostOffice { get; set; }

        [DisplayName("Police Station")]
        [Required]
        public string PermanentPoliceStation { get; set; }

        [DisplayName("Postal Code")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "must contain only digits.")]

        public int PermanentPostalCode { get; set; }

        [DisplayName("District")]
        [Required]
        public string PermanentDistrict { get; set; }

        [DisplayName("Home District")]
        [Required]
        public string HomeDistrict { get; set; }


        [DisplayName("Career Objective (max 900 char)")]
        [Required, MaxLength(900)]
        public string CareerObjective { get; set; }


        [DisplayName("MaritalStatus")]
        [Required]
        public string MaritalStatus { get; set; }

        [DisplayName("Religion")]
        [Required]
        public string Religion { get; set; }

        [DisplayName("Father's Name")]
        [Required]
        public string FathersName { get; set; }

        [DisplayName("Mothers's Name")]
        [Required]
        public string MothersName { get; set; }

        [DisplayName("Spouse Name")]
        public string? Spouse { get; set; }

        public int UserId { get; set; }

        [DisplayName("Do you have experience")]
        public string HasExperience { get; set; }


        [DisplayName("Computer Literacy")]
        [Required]
        public string ComputerLiteracy { get; set; }

    }
}
