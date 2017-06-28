﻿using Adopcat.Mobile.Helpers;
using Adopcat.Mobile.Models;
using Adopcat.Mobile.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Adopcat.Mobile.ViewModels
{
    public class MyPostersPageViewModel : BaseViewModel
    {
        private ObservableCollection<PosterOutput> _myPosters;
        public ObservableCollection<PosterOutput> MyPosters
        {
            get { return _myPosters; }
            set { SetProperty(ref _myPosters, value); }
        }

        public DelegateCommand<int?> SelectedPosterCommand { get; set; }
        public DelegateCommand<int?> DeleteCommand { get; set; }

        public MyPostersPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            Title = "Meus Anúncios";

            SelectedPosterCommand = new DelegateCommand<int?>(SelectedPosterCommandExecute);
            DeleteCommand = new DelegateCommand<int?>(DeleteCommandExecute);
        }

        private async void DeleteCommandExecute(int? posterId)
        {
            try
            {
                if (posterId.HasValue)
                {
                    var delete = await _dialogService.DisplayAlertAsync("Deletar", "Deseja realmente deletar este anúncio?", "Sim", "Não");

                    if (delete)
                    {
                        ShowLoading = true;
                        await App.ApiService.DeletePoster(posterId.Value, "bearer " + Settings.AuthToken);
                        await _dialogService.DisplayAlertAsync("Sucesso!", "Anúncio deletado com sucesso.", "Ok");

                        MyPosters.Remove(MyPosters.First(w => w.Id == posterId));
                    }
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.Handle(ex, true);
            }
            finally
            {
                ShowLoading = false;
            }           
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                MyPosters = new ObservableCollection<PosterOutput>(
                                    await App.ApiService.GetMyPosters("bearer " + Settings.AuthToken));

                foreach (var poster in MyPosters)
                {
                    poster.MainPictureUrl = poster.PetPictures.FirstOrDefault()?.Url;
                }
            }
            catch (Exception ex)
            {
                await ExceptionHandler.Handle(ex);
            }
        }

        private async void SelectedPosterCommandExecute(int? posterId)
        {
            if (posterId != null)
            {
                var parameters = new NavigationParameters
                {
                    { "posterId", posterId.Value }
                };
                await _navigationService.NavigateAsync(nameof(EditMyPosterPage), parameters);
            }
        }
    }
}
