﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Respositories;
using Orders.Shared.Entities;
using System.Diagnostics.Metrics;

namespace Orders.Frontend.Pages.States
{
    public partial class StateDetails
    {
        private int currentPage = 1;
        private int totalPages;
        private List<City>? cities;
        private State? state;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        [Parameter] public int StateId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadStateAsync();
            if (ok)
            {
                ok = await LoadCitiesAsync(page);
                if (ok)
                {
                    await LoadPagesAsync();
                }
            }
        }

        private async Task LoadPagesAsync()
        {
            var url = $"/api/cities/totalPages?id={StateId}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&Filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }

        private async Task<bool> LoadCitiesAsync(int page)
        {
            var url = $"/api/cities?id={StateId}&page={page}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&Filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<City>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            cities = responseHttp.Response;
            return true;
        }

        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }
        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async Task<bool> LoadStateAsync()
        {
            var responseHttp = await Repository.GetAsync<State>($"/api/states/{StateId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                    return false;
                }
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            state = responseHttp.Response;
            return true;
        }

        private async Task DeleteAsync(City city)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Estas seguro de querer borrar la ciudad: {city.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si",
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
                return;

            var responseHttp = await Repository.DeleteAsync<City>($"/api/cities/{city.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
                return;
            }
            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con éxito.");
        }

    }
}
