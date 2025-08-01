using Microsoft.Extensions.Localization;
using System.Text.Json;
using TaskForce.Client.EventAggregator;
using TaskForce.Localization.Resources;

namespace TaskForce.Client
{
    public class HttpErrorMessageHandler(ILogger<HttpErrorMessageHandler> logger, StateContainerService stateService, IStringLocalizer<ResourceLanguage> localizer) : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put ||
                    request.Method == HttpMethod.Delete)
                {
                    stateService.NotifyToast(ToastMessage.Success(localizer[ResourceLanguage.Successo]));
                }

                return response;
            }

            try
            {
                if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
                {
                    var json = await response.Content.ReadAsStringAsync(cancellationToken);
                    logger.LogError(json);
                    var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    var problemDetails = JsonSerializer.Deserialize<ProblemDetailsWithErrors>(json, jsonOptions);

                    switch (problemDetails?.Status)
                    {
                        case 401:
                            // Case managed by UI
                            break;
                        case 400:
                            var businessMessage = problemDetails.Detail is not null ? ErrorMessages.ResourceManager.GetString(problemDetails.Detail) : null;
                            stateService.NotifyToast(ToastMessage.Warning(businessMessage ?? problemDetails.Title));
                            break;
                        case 403:
                            stateService.NotifyToast(ToastMessage.Error(localizer[ResourceLanguage.AccessoNegato]));
                            break;
                        case 409:
                            stateService.NotifyToast(ToastMessage.Error($"{localizer[ResourceLanguage.ElementoDuplicato]}: {problemDetails.Detail}"));
                            break;
                        case 404:
                            stateService.NotifyToast(ToastMessage.Error(localizer[ResourceLanguage.NonTrovato]));
                            break;
                        default:
                            stateService.NotifyToast(ToastMessage.Error(problemDetails.Detail ?? problemDetails.Title));
                            break;
                    }
                }
                else
                {
                    logger.LogError(response.ReasonPhrase);
                    stateService.NotifyToast(ToastMessage.Error(response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                stateService.NotifyToast(ToastMessage.Error(ex.Message));
            }

            return response;

        }
    }
}
