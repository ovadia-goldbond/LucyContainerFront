using Blazored.Modal.Services;
using LucyContainerFront.Pages.Modals;
using LucyShared.Classes;
using LucyShared.Models;
using LucyShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
namespace LucyContainerFront.Pages
{
    public partial class Login : ComponentBase
    {
        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [Inject]
        private  IAuthService AuthService { get; set;} = null!;
        [Inject]
        private  NavigationManager Navigation { get; set; } = null!;

        public LoginDto LoginModel = new();
        public string? ErrorMessage = String.Empty;

        public async Task UserLoginAsync()
        {
            ErrorMessage = null;

            var response = await AuthService.LoginEmpBoolAsync(LoginModel);
            if (!response)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }
            Navigation.NavigateTo("/ContainerSearch");
        }

        public void Logout()
        {
            AuthState.Token = null;
            AuthState.Role = null;
            Navigation.NavigateTo("/login", forceLoad: true);
        }
        public async void ShowMovieModalAsync()
        {
            var moviesModal = Modal.Show<Movies>("My Movies");
            var result = await moviesModal.Result;
            if (result.Cancelled)
            {
                Console.WriteLine("Modal was cancelled");
            }
            else if (result.Confirmed)
            {
                Console.WriteLine("Modal was closed");
            }
        }
    }
}
