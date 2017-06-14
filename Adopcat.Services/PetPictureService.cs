﻿using System;
using System.Threading.Tasks;
using Adopcat.Model;
using Adopcat.Services.Interfaces;
using Adopcat.Data.Interfaces;

namespace Adopcat.Services
{
    public class PetPictureService : BaseService, IPetPictureService
    {
        private IBlobStorageService _blobStorageService;
        private IPetPictureRepository _petPictureRepository;

        public PetPictureService(ILoggingService log, 
                                 IBlobStorageService blobStorageService,
                                 IPetPictureRepository petPictureRepository) : base(log)
        {
            _blobStorageService = blobStorageService;
            _petPictureRepository = petPictureRepository;
        }

        public async Task<PetPicture> Create(byte[] picture, int posterId)
        {
            return await TryCatch(async () =>
            {
                var petPictureBlobStorageUrl = await _blobStorageService.AddImageToBlobStorageAsync(picture);

                var petPicture = new PetPicture
                {
                    PosterId = posterId,
                    Url = petPictureBlobStorageUrl
                };

                return await _petPictureRepository.CreateAsync(petPicture);
            });
        }
    }
}