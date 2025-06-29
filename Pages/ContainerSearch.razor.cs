using Blazored.Modal;
using Blazored.Modal.Services;
using LucyContainerFront.Pages.Modals;
using LucyShared.Models;
using LucyShared.Services;
using Microsoft.AspNetCore.Components;


namespace LucyContainerFront.Pages
{
    public partial class ContainerSearch : ComponentBase
    {
        [CascadingParameter] IModalService DestinationModal { get; set; } = default!;
        [Inject]
        private  IContainerService ContainerService { get; set; } = null!;
        [Inject]
        private  NavigationManager Cavigation { get; set; } = null!;
        [Inject]
        private  IConfiguration Configuration { get; set; } = null!;

        public StringDto ContainerSearchModel = new();
        public string? ErrorMessage;
        public List<StringDto> ContainersList = null!;
        public ContainerData? ContainerData = null;
        public DestinationData? Destination = new();
        public string Terminal = string.Empty;
        protected override Task OnInitializedAsync()
        {
            Terminal = Configuration.GetValue<string>("Terminal") ?? string.Empty;
            return base.OnInitializedAsync();
        }
        public async Task SearchAsync()
        {
            ContainerData = null;
            ContainersList = await ContainerService.GetContainersDataListAsync(ContainerSearchModel.Data, Terminal);
            //StateHasChanged();
        }
        public async Task LoadContainerDataAsync(string selected)
        {
            //ContainerData = new();
            ContainerData = await ContainerService.GetContainerDataAsync(selected, Terminal);
        }
        public async Task SetContainerAVDMAsync(string avdm)
        {
            ContainerData!.AVDM = avdm;
            Destination = await ContainerService.GetDestinationDataAsync(ContainerData);
            ShowDestinationModalAsync();
        }
        public async void ShowDestinationModalAsync()
        {
            var parameters = new ModalParameters()
                .Add(nameof(Destination), Destination);
            var destinationModal = DestinationModal.Show<DestinationModal>("Destination",parameters);
            var result = await destinationModal.Result;
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
