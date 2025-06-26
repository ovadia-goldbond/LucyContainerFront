using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

public abstract class ViewModelComponentBase<TVm> : ComponentBase, IDisposable
    where TVm : class, INotifyPropertyChanged
{
    /// <summary>
    /// Inject your ViewModel here. 
    /// Blazor will resolve it from DI.
    /// </summary>
    [Inject] protected TVm ViewModel { get; set; } = default!;

    protected override void OnInitialized()
    {
        // Subscribe to *all* property changes
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Any time *any* property changes, re-render the component.
        // You could check e.PropertyName if you only care about specific props.
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
}