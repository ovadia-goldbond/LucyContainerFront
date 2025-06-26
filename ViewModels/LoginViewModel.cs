using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LucyShared.Classes;
using LucyShared.Models;
using LucyShared.Services;
using Microsoft.AspNetCore.Components;
namespace LucyContainerFront.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigation;

        [ObservableProperty] private LoginDto loginModel = new();
        [ObservableProperty] private string? errorMessage;

        public LoginViewModel(IAuthService authService, NavigationManager navigation)
        {
            _authService = authService;
            _navigation = navigation;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = null;

            var response = await _authService.LoginEmpBoolAsync(LoginModel);
            if (!response)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }
            _navigation.NavigateTo("/ContainerSearch");
        }

        [RelayCommand]
        private void Logout()
        {
            AuthState.Token = null;
            AuthState.Role = null;
            _navigation.NavigateTo("/login", forceLoad: true);
        }
    }
}