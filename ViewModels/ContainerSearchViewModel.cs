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
    public partial class ContainerSearchViewModel : ObservableObject
    {
        private readonly IContainerService _containerService;
        private readonly NavigationManager _navigation;
        private readonly IConfiguration _configuration;
        [ObservableProperty] private StringDto containerSearchModel = new();
        [ObservableProperty] private string? errorMessage;
        [ObservableProperty] private List<StringDto> containersList = new();
        [ObservableProperty] private ContainerData containerData = null;
        private string terminal = string.Empty;
        public ContainerSearchViewModel(IContainerService containerService, 
                                        NavigationManager navigation,
                                        IConfiguration configuration)
        {
            _containerService = containerService;
            _navigation = navigation;
            _configuration = configuration;
            terminal = _configuration.GetValue<string>("Terminal") ?? string.Empty;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            ContainersList = await _containerService.GetContainersDataListAsync(containerSearchModel.Data, terminal);
        }
        [RelayCommand]
        private async Task LoadContainerDataAsync(string selected)
        {
            ContainerData = new();
            ContainerData  = await _containerService.GetContainerDataAsync(selected, terminal);
        }
    }
}