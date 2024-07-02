using API.Errors;
using API.Extensions;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces.Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Subscription = Stripe.Subscription;

namespace API.Controllers
{

    [Route("webhook")]
    public class WebhookController : BaseApiController
    {

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private readonly string _whSecret;
        private readonly ILogger<PaymentsController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPricesService _pricesService;
        private readonly ISubscriptionsService _subscriptionsService;


        public WebhookController(ILogger<PaymentsController> logger,
                    MyAwsCredentials credentials, UserManager<AppUser> userManager, IPricesService pricesService, ISubscriptionsService subscriptionsService)
        {
            _logger = logger;
            _userManager = userManager;
            _pricesService = pricesService;
            // _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
            _whSecret = credentials.WhSecret;
            _subscriptionsService = subscriptionsService;
        }

        [HttpPost]
        [NonAction]
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            //if (user == null)
            //{
            //    return NotFound(new ApiResponse(404));
            //}

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                Subscription subscription;
                SetupIntent setupIntent;

                if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    //if (user.Subscription != null)
                    //{
                    //    user.Subscription = null;
                    //    await _userManager.UpdateAsync(user);
                    //}
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    subscription = (Subscription)stripeEvent.Data.Object;
                    //await CreateOrUpdateSubscription(user, subscription);

                    _logger.LogInformation("Subscription updated: ", subscription.Id);

                }
                else if (stripeEvent.Type == Events.InvoicePaid)
                {
                    var invoice = stripeEvent.Data.Object as Invoice;

                    //await _subscriptionsService.CreateUserSubscriptionAsync(invoice.CustomerId, invoice.Subscription.Items.Data[0].Price.Id);
                }
                else if (stripeEvent.Type == Events.InvoicePaymentFailed)
                {
                }
                else if (stripeEvent.Type == Events.InvoiceUpdated)
                {
                }
                //else if (stripeEvent.Type == Events.InvoiceWillBeDue)
                //{
                //}
                else if (stripeEvent.Type == Events.SetupIntentCreated)
                {
                    setupIntent = (SetupIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Intent updated: ", setupIntent.Id);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        private async Task CreateOrUpdateSubscription(AppUser user, Subscription subscription)
        {
            if (subscription != null && user.Subscription == null)
            {
                user.Subscription = new()
                {
                    Id = subscription.Id,
                    Price = subscription.Items.FirstOrDefault().Price.UnitAmountDecimal,
                    PlanType = subscription.Items.FirstOrDefault().Price.Type,
                    Description = subscription.Items.FirstOrDefault().Plan.Product.Description,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.CurrentPeriodStart
                };
                await _userManager.UpdateAsync(user);
            }
        }
    }
}

