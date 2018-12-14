using ApiHessapp.DTO.TFA;
using Google.Authenticator;
using System;
using System.Net;
using System.Web.Http;

namespace ApiHessapp.Controllers
{
    public class TFAController : ApiController
    {
        [HttpPost]
        public IHttpActionResult NewQRCode(TFAQRRequestDTO nc)
        {
            if (ModelState.IsValid)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                var setupInfo = tfa.GenerateSetupCode("Google Auth", "Hessapp-TFA", "f729877b2b104d0eb717610e9cf7a532-" + nc.nickname, 300, 300);
                TFAQRResponseDTO rsp = new TFAQRResponseDTO();
                rsp.qrurl = setupInfo.QrCodeSetupImageUrl;
                rsp.key = setupInfo.ManualEntryKey;
                return Content(HttpStatusCode.OK, rsp);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public IHttpActionResult AuthControl(TFAuthRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                bool isValid = tfa.ValidateTwoFactorPIN("f729877b2b104d0eb717610e9cf7a532-"+ model.nickname, model.pin);
                TFAuthResponseDTO valid = new TFAuthResponseDTO();
                valid.isValid= isValid;
                if (isValid)
                {
                    return Content(HttpStatusCode.OK,valid);
                }
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
       
    }
}
