﻿using Adopcat.Data.Interfaces;
using Adopcat.Model;
using Adopcat.Model.Enums;
using Adopcat.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Adopcat.Services
{
    public class LoggingService : ILoggingService
    {
        private ISystemLogRepository _systemLogRepository;

        public LoggingService(ISystemLogRepository systemLogRepository)
        {
            _systemLogRepository = systemLogRepository;
        }

        public async Task Error(Exception ex)
        {
            try
            {
                var error = new SystemLog()
                {
                    LogDate = DateTime.Now,
                    LogType = ELogType.Error,
                    Text = ex.Message,
                };

                await _systemLogRepository.CreateAsync(error);
            }
            catch (Exception) { }
        }
    }
}
