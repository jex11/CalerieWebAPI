using AdobeESignWebAPI.Model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AdobeESignWebAPI.Services
{
    public class WebAPIClient
    {
        private readonly HttpClient _httpClient;

        private readonly WebAPISetting _webApiSetting;

        public WebAPIClient(IOptionsSnapshot<WebAPISetting> webApiSetting)
        {
            _webApiSetting = webApiSetting.Value;
          
            var handler = new HttpClientHandler
            {
                //UseDefaultCredentials = true,
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_webApiSetting.ApiUrl),
                Timeout = TimeSpan.FromMinutes(3)
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _webApiSetting.AccessToken);
        }

        public async Task<string> UploadTransientDoc(byte[] fileByteArr, string fileName)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Create ByteArrayContent for the byte array
                var byteArrayContent = new ByteArrayContent(fileByteArr);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Add the ByteArrayContent to the MultipartFormDataContent
                content.Add(byteArrayContent, "File", fileName);

                // Send the POST request
                var response = await _httpClient.PostAsync("api/rest/v6/transientDocuments", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var test = JsonSerializer.Deserialize<TransientDocumentResponseModel>(result);
                    return test.transientDocumentId;
                }
                else
                {
                    Console.WriteLine("File upload failed. Status code: " + response.StatusCode);
                    return await response.Content.ReadAsStringAsync();
                }
            }      
        }


        public async Task<string> GetAgreementAsync()
        {
            var response = await _httpClient.GetAsync("/api/rest/v6/agreements");

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> PostAgreement(string transientDocID, string email)
        {
            PostAgreementRequestModel model = new PostAgreementRequestModel
            {
                name = "Calerie Invoice",
                signatureType = "ESIGN",
                state = "IN_PROCESS",
                fileInfos = new List<FileInfosModel> { new FileInfosModel { transientDocumentId = transientDocID } },
                participantSetsInfo = new List<ParticipantSetsInfoModel> { new ParticipantSetsInfoModel { order = 1, role = "SIGNER", memberInfos = new List<MemberInfosModel> { new MemberInfosModel { email = email } } } },
            };
          
            var response = await _httpClient.PostAsJsonAsync("api/rest/v6/agreements", model);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var contentStr = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PostAgreementResponseModel>(contentStr);
                return result.id;
            }
            else
            {
                Console.WriteLine("File upload failed. Status code: " + response.StatusCode);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetSigningUrls(string agreementID)
        {
            var response = await _httpClient.GetAsync($"api/rest/v6/agreements/{agreementID}/signingUrls");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AgreementSigningUrlsResponseModel>(content);

            return result.signingUrlSetInfos.FirstOrDefault().signingUrls.FirstOrDefault().esignUrl;
        }

        public async Task<string> ViewAgreement(string agreementID, bool autoLoginUser = false)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/rest/v6/agreements/{agreementID}/views", new { name = "ALL", commonViewConfiguration = new { autoLoginUser = autoLoginUser, noChrome = true } });
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var contentStr = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AgreementViewResponseModel>(contentStr);
                return result.agreementViewList.FirstOrDefault().url;
            }
            else
            {
                Console.WriteLine("File upload failed. Status code: " + response.StatusCode);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
