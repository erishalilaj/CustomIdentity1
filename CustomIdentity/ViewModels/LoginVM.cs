using System.ComponentModel.DataAnnotations;

namespace CustomIdentity.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
//< div class= "row" >Login.chtml Register 


// <div class="col-8">
// 	<a asp-action="Register" class="text-decoration-none float-start mt-2">Don't have account? </a>
// </div>
//                    < div class= "col-4" >

//                        < input type = "submit" value = "Login" class= "btn btn-primary btn-sm float-end" />

//                    </ div >

//                </ div >