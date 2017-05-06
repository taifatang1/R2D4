using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GlobalResources;
using Newtonsoft.Json;
using R2D4Web.Enums;
using R2D4Web.Models;

namespace R2D4Web.Controllers
{
    public class FacebookController : Controller
    {
        public ActionResult Receive()
        {
            var hub = new Hub(Request.QueryString);

            if (hub.Mode == HubMode.Subscribe &&
                hub.VerifyToken == FacebookConfig.VerifyToken)
            {
                return Json(hub.Challenge, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        [ActionName("receive")]
        [HttpPost]
        public ActionResult ReceivePost(BotRequest request)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var entry in request.Entry)
                {
                    foreach (var message in entry.Messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.Message?.Text))
                            continue;

                        var msg = "You said: " + message.Message.Text;
                        var json = $@" {{recipient: {{  id: {message.Sender.Id}}},message: {{text: ""{msg}"" }}}}";
                        PostRaw(FacebookConfig.AccessTokenUrl + FacebookConfig.PageAccessToken, json);
                    }
                }
            });
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        private string PostRaw(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
    }

    public class BotRequest
    {
        public string @Object { get; set; }
        public List<BotEntry> Entry { get; set; }
    }

    public class BotEntry
    {
        public string Id { get; set; }
        public long Time { get; set; }
        public List<BotMessageReceivedRequest> Messaging { get; set; }
    }

    public class BotMessageReceivedRequest
    {
        public BotUser Sender { get; set; }
        public BotUser Recipient { get; set; }
        public string Timestamp { get; set; }
        public BotMessage Message { get; set; }
        public BotPostback Postback { get; set; }
    }

    public class BotPostback
    {
        public string Payload { get; set; }
    }


    public class BotMessageResponse
    {
        public BotUser Recipient { get; set; }
        public MessageResponse Message { get; set; }
    }

    public class BotMessage
    {
        public string Mid { get; set; }
        public List<MessageAttachment> Attachments { get; set; }
        public long Seq { get; set; }
        public string Text { get; set; }
        [JsonProperty("quick_reply")]
        public QuickReply QuickReply { get; set; }
    }

    public class BotUser
    {
        public string Id { get; set; }
    }

    public class MessageResponse
    {
        public MessageAttachment Attachment { get; set; }
        [JsonProperty("quick_replies")]
        public List<QuickReply> QuickReplies { get; set; }
        public string Text { get; set; }
    }

    public class QuickReply
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        public string Title { get; set; }
        public string Payload { get; set; }
    }

    public class ResponseButtons
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Payload { get; set; }

        public string Url { get; set; }
        [JsonProperty("webview_height_ratio")]
        public string WebviewHeightRatio { get; set; }
    }

    public class MessageAttachment
    {
        public string Type { get; set; }
        public MessageAttachmentPayLoad Payload { get; set; }
    }

    public class MessageAttachmentPayLoad
    {
        public string Url { get; set; }
        [JsonProperty("template_type")]
        public string TemplateType { get; set; }
        [JsonProperty("top_element_style")]
        public string TopElementStyle { get; set; }
        public List<PayloadElements> Elements { get; set; }
        public List<ResponseButtons> Buttons { get; set; }
        [JsonProperty("recipient_name")]
        public string RecipientName { get; set; }
        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }
        public string Currency { get; set; }
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
        [JsonProperty("order_url")]
        public string OrderUrl { get; set; }
        public string Timestamp { get; set; }
        public Address Address { get; set; }
        public Summary Summary { get; set; }
    }

    public class PayloadElements
    {
        public string Title { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        public string Subtitle { get; set; }
        public List<ResponseButtons> Buttons { get; set; }
        [JsonProperty("item_url")]
        public string ItemUrl { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
    }

    public class Address
    {
        [JsonProperty("street_2")]
        public string Street2 { get; set; }
        [JsonProperty("street_1")]
        public string Street1 { get; set; }
        public string City { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class Summary
    {
        public decimal? Subtotal { get; set; }
        [JsonProperty("shipping_cost")]
        public decimal? ShippingCost { get; set; }
        [JsonProperty(PropertyName = "total_tax")]
        public decimal? TotalTax { get; set; }
        [JsonProperty(PropertyName = "total_cost")]
        public decimal TotalCost { get; set; }
    }
}