﻿using MeAdota.Mobile.Helpers;
using MeAdota.Mobile.Models;
using MeAdota.Mobile.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MeAdota.Mobile.ViewModels
{
    public class LoadingPostersPageViewModel : BaseViewModel
    {
        private bool _hasNoPosters;
        public bool HasNoPosters
        {
            get { return _hasNoPosters; }
            set { SetProperty(ref _hasNoPosters, value); }
        }

        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand FilterCommand { get; set; }

        public LoadingPostersPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Title = "Anúncios";

            RefreshCommand = new DelegateCommand(RefreshCommandExecute);
            FilterCommand = new DelegateCommand(FilterCommandExecute);
        }

        private async Task LoadPosters(NavigationParameters parameters)
        {
            ShowLoading = true;
            try
            {
                var posters = new List<PosterOutput>();

                Filter filter = null;
                if (parameters.Any(a => a.Key.Equals("filter")))
                {
                    filter = parameters.GetValue<Filter>("filter");
                }

                if (filter != null)
                {
                    posters = await App.ApiService.GetFilteredPosters(filter.City ?? string.Empty,
                                        filter.PetType, filter.Castrated, filter.Dewormed, "bearer " + Settings.AuthToken);
                }
                else
                {
                    posters = await App.ApiService.GetPosters("bearer " + Settings.AuthToken);
                }

                if (!posters.Any())
                {
                    HasNoPosters = true;
                    return;
                }

                HasNoPosters = false;
                foreach (var poster in posters)
                {
                    poster.MainPictureUrl = poster.PetPictures.FirstOrDefault()?.Url;
                }

                var posterPageParameters = new NavigationParameters
                {
                    { "posters", posters },
                    { "filter", filter }
                };

                await _navigationService.NavigateAsync($"app:///{nameof(MenuPage)}/NavigationPage/{nameof(PostersPage)}", posterPageParameters);
            }            
            catch (Exception ex)
            {
                if (ex is Refit.ApiException)
                {
                    var refitEx = ex as Refit.ApiException;
                    if (refitEx.ReasonPhrase.Equals("Unauthorized"))
                    {
                        await App.MobileService.LogoutAsync();
                        await _navigationService.NavigateAsync($"app:///NavigationPage/{nameof(LoginPage)}");
                    }
                }
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                ShowLoading = false;
            }
        }

        private async void RefreshCommandExecute()
        {
            await LoadPosters(new NavigationParameters());
        }

        private async void FilterCommandExecute()
        {
            await _navigationService.NavigateAsync($"NavigationPage/{nameof(FilterPage)}", null, true);
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await LoadPosters(parameters);
        }
    }
}