﻿using AdobeESignWebAPI.Model;
using AdobeESignWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime;

namespace AdobeESignWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransientDocController : ControllerBase
    {
        private readonly WebAPISetting _webApiSetting;
        private readonly WebAPIClient _webAPIClient;

        public TransientDocController(IOptionsSnapshot<WebAPISetting> webApiSetting, WebAPIClient webAPIClient)
        {
            _webApiSetting = webApiSetting.Value;
            _webAPIClient = webAPIClient;
        }

        [HttpGet("agreement")]
        public async Task<IActionResult> Agreement()
        {
            try
            {
                var result = await _webAPIClient.GetAgreementAsync();
                return new JsonResult(new { issuccess = true, message = "Success", data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { issuccess = false, message = "Error", data = ex.Message });
            }            
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequestModel model)
        {
            try
            {
                byte[] uploadedInvoiceByteArr;
                string transientDocID = "";
                string agreementID = "";
                string docUrl = "";
                if (model.file == null || model.file.Length == 0)
                    return BadRequest("No file was uploaded.");

                if(string.IsNullOrWhiteSpace(model.email))
                    return BadRequest("No file was uploaded.");

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", model.file.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    // Copy the uploaded file's stream into the memory stream
                    await model.file.CopyToAsync(memoryStream);
                    uploadedInvoiceByteArr = memoryStream.ToArray();  // Convert the memory stream into a byte array

                    transientDocID = await _webAPIClient.UploadTransientDoc(uploadedInvoiceByteArr, model.file.FileName);

                    agreementID = await _webAPIClient.PostAgreement(transientDocID, model.email);

                    //docUrl = await _webAPIClient.ViewAgreement(agreementID);
                }

                return new JsonResult(new { issuccess = true, message = "Success", data = agreementID });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { issuccess = false, message = "Error", data = ex.Message });
            }
            
        }

        [HttpPost]
        [Route("view_agreement_signingUrls")]
        public async Task<IActionResult> ViewAgreementSigningUrlsByID(string agreementID)
        {
            try
            {
                string signUrl = "";

                signUrl = await _webAPIClient.GetSigningUrls(agreementID);

                return new JsonResult(new { issuccess = true, message = "Success", data = signUrl });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { issuccess = false, message = "Error", data = ex.Message });
            }
            
        }
    }
}
