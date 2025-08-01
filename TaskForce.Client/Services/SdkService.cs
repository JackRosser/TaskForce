using FluentResults;
using System.Runtime.CompilerServices;
using TaskForceSdk;
using Exception = System.Exception;

namespace TaskForce.Client.Services
{
    public class SdkService(
        FasiProgettoClient fasiProgettoClient,
        MacroFasiClient macroFasiClient,
        PauseClient pauseClient,
        PreseInCaricoClient preseInCaricoClient,
        ProgettiClient progettiClient,
        UsersClient usersClient,
        UsersFasiClient usersFasiClient,
        ILogger<SdkService> logger)
    {

        /// <summary>
        /// Esegue la richiesta http e ritorna il risultato tramite Result pattern
        /// </summary>
        public async Task<Result<T>> SendRequestAsync<T>(Func<FasiProgettoClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(fasiProgettoClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<MacroFasiClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(macroFasiClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<PauseClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(pauseClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<PreseInCaricoClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(preseInCaricoClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<UsersClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(usersClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<ProgettiClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(progettiClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<T>> SendRequestAsync<T>(Func<UsersFasiClient, Task<T>> action, [CallerMemberName] string caller = "")
        {
            try
            {
                return Result.Ok(await action(usersFasiClient));
            }
            catch (Exception ex)
            {
                logger.LogError($"Caller {caller},{ex.Message}");
                return Result.Fail(ex.Message);
            }
        }


    }
}
