using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Stripe;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackExchange.Redis;
using Stripe;
using System.Collections.Generic;

namespace API.Controllers
{
    [Authorize]
    public class StripeController(IProductsService productsService,
        IPaymentMethodsService paymentMethodsService,
        ICustomersService customersService,
        IPricesService pricesService,
        IInvoicesService invoicesService,
        ISubscriptionsService subscriptionService,
        UserManager<AppUser> userManager,
        IMapper mapper) : BaseApiController
    {
        private readonly IProductsService _productsService = productsService;
        private readonly ICustomersService _customersService = customersService;
        private readonly IPricesService _pricesService = pricesService;
        private readonly IInvoicesService _invoicesService =  invoicesService;
        private readonly ISubscriptionsService _subscriptionService = subscriptionService;
        private readonly IPaymentMethodsService _paymentMethodsService = paymentMethodsService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;


        [HttpGet("getproducts/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<StripeProductDto>>> GetProducts(int take)
        {
            try
            {
                var products = await _productsService.GetProductsAsync(take);

                var tasks = products.Data.Select(p => _pricesService.GetPriceAsync(p.DefaultPriceId));
                var prices = await Task.WhenAll(tasks);

                var productsWithPrices = products.Data.Select(p => new StripeProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = prices.FirstOrDefault(pr => pr.Id == p.DefaultPriceId)?.UnitAmount
                }).ToList();

                return Ok(productsWithPrices);
            } catch(StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        //[Cached(600)]
        [HttpGet("getproduct/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StripeProductDto>> GetProduct(string id)
        {
            try
            {
                var product = await _productsService.GetProductAsync(id);
                return Ok(_mapper.Map<StripeProductDto>(product));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("postproduct")]
        public async Task<ActionResult<StripeProductDto>> PostProduct(string name)
        {
            try
            {
                var product = await _productsService.CreateProductAsync(name);
                return _mapper.Map<StripeProductDto>(product);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("deleteproduct")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            try
            {
                var product = await _productsService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getprices/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<StripePriceDto>>> GetPrices(int take)
        {
            try
            {
                var prices = await _pricesService.GetPricesAsync(take);
                return Ok(_mapper.Map<IReadOnlyList<StripePriceDto>>(prices.Data));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getprice/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StripePriceDto>> GetPrice(string id)
        {
            try
            {
                var price = await _pricesService.GetPriceAsync(id);
                return Ok(_mapper.Map<StripePriceDto>(price));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("postprice")]
        public async Task<ActionResult<StripePriceDto>> PostPrice(StripeCreatePriceDto createPriceDto)
        {
            try
            {
                var price = await _pricesService.CreatePriceAsync(createPriceDto.ProductId, createPriceDto.Currency, 
                                                        createPriceDto.UnitAmount, createPriceDto.Interval);
                return _mapper.Map<StripePriceDto>(price);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("updateprice")]
        public async Task<ActionResult<StripePriceDto>> PutPrice(string id, Dictionary<string, string> metadata)
        {
            try
            {
                var price = await _pricesService.UpdatePriceAsync(id, metadata);
                return _mapper.Map<StripePriceDto>(price);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


        [HttpGet("getinvoices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<StripeInvoiceDto>>> GetInvoices()
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user == null)
                {
                    return NotFound(new ApiResponse(401));
                }

                if (string.IsNullOrEmpty(user.CustomerId))
                {
                    return BadRequest(new ApiResponse(400, "User does not have a stripe account."));
                }

                var invoicesForCustomer = await _invoicesService.GetInvoicesForCustomerAsync(user.CustomerId);

                return Ok(_mapper.Map<IReadOnlyList<StripeInvoiceDto>>(invoicesForCustomer.Data));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getinvoice/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StripeInvoiceDto>> GetInvoice(string id)
        {
            try
            {
                var invoice = await _invoicesService.GetInvoiceAsync(id);
                return Ok(_mapper.Map<StripeInvoiceDto>(invoice));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getcustomers/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<StripeCustomerDto>>> GetCustomers(int take)
        {
            try
            {
                var customers = await _customersService.GetCustomersAsync(take);
                return Ok(_mapper.Map<IReadOnlyList<StripeCustomerDto>>(customers.Data));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        //[Cached(600)]
        [HttpGet("getcustomerbyemail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StripeCustomerDto>> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _customersService.GetCustomerByEmailAsync(email);
                return Ok(_mapper.Map<StripeCustomerDto>(customer));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("postcustomer")]
        public async Task<ActionResult<StripeCustomerDto>> PostCustomer(string name, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404, "User was not found"));
                }

                if (!string.IsNullOrEmpty(user.CustomerId))
                {
                    return BadRequest(new ApiResponse(400, "This user already has a stripe account."));
                }

                var customer = await _customersService.CreateCustomerAsync(name, email);
                if(customer == null)
                {
                    return BadRequest(new ApiResponse(400, "Could not create a stripe customer."));
                }

                user.CustomerId = customer.Id;
                await _userManager.UpdateAsync(user);

                return _mapper.Map<StripeCustomerDto>(customer);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("deletecustomer")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            try
            {
                var customer = await _customersService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("updatecustomer")]
        public async Task<ActionResult<StripeCustomerDto>> PutCustomer(string id, Dictionary<string, string> metadata)
        {
            try
            {
                var customer = await _customersService.UpdateCustomerAsync(id, metadata);
                return _mapper.Map<StripeCustomerDto>(customer);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("createsetupintent")]
        public async Task<ActionResult<SetupIntentDto>> CreateSetupIntent([BindRequired]string paymentMethodId, string returnUrl)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404));
                }

                var setupIntent = await _paymentMethodsService.CreateSetupIntent(user.CustomerId, paymentMethodId, returnUrl);

                return _mapper.Map<SetupIntentDto>(setupIntent);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("attachpaymentmethodtocustomer")]
        public async Task<ActionResult<PaymentMethodDto>> AttachPaymentMethodToCustomer([BindRequired] string customerId, [BindRequired] string paymentMethodId)
        {
            try
            {
                var paymentMethod = await _paymentMethodsService.AttachPaymentMethodToCustomerAsync(customerId, paymentMethodId);

                return _mapper.Map<PaymentMethodDto>(paymentMethod);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


        [HttpGet("getuserpaymentmethods/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<PaymentMethodDto>>> GetUserPaymentMethods(int take)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user == null)
                {
                    return NotFound(new ApiResponse(401));
                }

                if (string.IsNullOrEmpty(user.CustomerId))
                {
                    return BadRequest(new ApiResponse(400, "User does not have a stripe account."));
                }

                var paymentMethods = await _paymentMethodsService.GetCustomerPaymentMethodsAsync(user.CustomerId, take);
                return Ok(_mapper.Map<IReadOnlyList<PaymentMethodDto>>(paymentMethods.Data));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        //[HttpGet("getcustomerpaymentmethod/{customerId}/{paymentmethodId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<PaymentMethodDto>> GetCustomerPaymentMethod(string customerId, string paymentmethodId)
        //{
        //    try
        //    {
        //        var paymentMethod = await _paymentMethodsService.GetCustomerPaymentMethodAsync(customerId, paymentmethodId);
        //        return Ok(_mapper.Map<PaymentMethodDto>(paymentMethod));
        //    }
        //    catch (StripeException ex)
        //    {
        //        return BadRequest(new ApiResponse(400, ex.Message));
        //    }
        //}

        [HttpPut("updatepaymentmethod")]
        public async Task<ActionResult<PaymentMethodDto>> PutPaymentMethod(string id, Dictionary<string, string> metadata)
        {
            try
            {
                var paymentMethod = await _paymentMethodsService.UpdatePaymentMethodAsync(id, metadata);
                return _mapper.Map<PaymentMethodDto>(paymentMethod);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("detachpaymentmethodfromcustomer")]
        public async Task<ActionResult> DetachPaymentMethodFromCustomer(string id)
        {
            try
            {
                var paymentMethod = await _paymentMethodsService.DetachPaymentMethodFromCustomerAsync(id);
                return NoContent();
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getsubscription/{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetSubscription(string id)
        {
            try
            {
                var subscription = await _subscriptionService.GetSubscriptionAsync(id);
                return Ok(_mapper.Map<SubscriptionDto>(subscription));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getusersubscription")]
        public async Task<ActionResult<SubscriptionDto>> GetUserSubscription()
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404));
                }

                return Ok(_mapper.Map<SubscriptionDto>(user.Subscription));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("getsubscriptions/{take}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SubscriptionDto>>> GetSubscriptions(int take)
        {
            try
            {
                var subscriptions = await _subscriptionService.GetSubscriptionsAsync(take);
                return Ok(_mapper.Map<IReadOnlyList<SubscriptionDto>>(subscriptions.Data));
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("createsubscription")]
        public async Task<ActionResult<SubscriptionDto>> CreateSubscription(string customerId, string priceId)
        {
            try
            {
                var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User, default);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404));
                }

                if(user.Subscription != null) /*|| user?.Subscription?.Status == "canceled")*/
                {
                    return BadRequest(new ApiResponse(400, $"{user.Email} already has a Subscription."));
                }

                var subscription = await _subscriptionService.CreateSubscriptionAsync(customerId, priceId);
                var price = await _pricesService.GetPriceAsync(priceId);

                user.Subscription = new()
                {
                    Id = subscription.Id,
                    StartDate = subscription.Created,
                    EndDate = subscription.CurrentPeriodEnd,
                    Status = subscription.Status,
                    CancelAtPeriodEnd = subscription.CancelAtPeriodEnd,
                    SubscriptionItemId = subscription.Items.Data[0].Id,
                    Description  = price.Product.Description,
                    PlanType = price.Product.Name,
                    Price = price.UnitAmountDecimal
                };

                var result = await _userManager.UpdateAsync(user);
                return _mapper.Map<SubscriptionDto>(subscription);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("completesubscription")]
        public async Task<ActionResult<SubscriptionDto>> CompleteSubscriptionAsync(string paymentIntentId)
        {
            try
            {
                var result = await _subscriptionService.CompleteSubscriptionAsync(paymentIntentId);
                if (result == null)
                {
                    return BadRequest(new ApiResponse(400, "Payment intent could not be completed."));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("createsubscriptionintent")]
        public async Task<ActionResult<SubscriptionCreateResponseDto>> CreateSubscriptionIntent(string priceId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            try
            {
                var subscription = await _subscriptionService.CreateSubscriptionIntentAsync(priceId, user.CustomerId);
                return new SubscriptionCreateResponseDto
                {
                    SubscriptionId = subscription.Id,
                    ClientSecret = subscription.LatestInvoice.PaymentIntent.ClientSecret,
                };
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("addsubscriptiontouser")]
        public async Task<ActionResult<SubscriptionDto>> AddSubscriptionToUser(string subscriptionId, string priceId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            try
            {
                var subscription = await _subscriptionService.GetSubscriptionAsync(subscriptionId);
                var price = await _pricesService.GetPriceAsync(priceId);

                if (subscription != null && subscription.Status == "active")
                {
                    var userSubscription = new Core.Entities.Identity.Subscription
                    {
                        Id = subscription.Id,
                        StartDate = subscription.Created,
                        EndDate = subscription.CurrentPeriodEnd,
                        Status = subscription.Status,
                        CancelAtPeriodEnd = subscription.CancelAtPeriodEnd,
                        SubscriptionItemId = subscription.Items.Data[0].Id,
                        Price = price.UnitAmountDecimal,
                        Description = price.Product.Description,
                        PlanType = price.Product.Name
                    };
                    user.Subscription = userSubscription;
                    await _userManager.UpdateAsync(user);
                }

                return _mapper.Map<SubscriptionDto>(subscription);

            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }



        [HttpPut("updatesubscription")]
        public async Task<ActionResult<SubscriptionDto>> PutSubscription(string id, Dictionary<string, string> metadata)
        {
            try
            {
                var subscription = await _subscriptionService.UpdateSubscriptionAsync(id, metadata);
                return _mapper.Map<SubscriptionDto>(subscription);
            }
            catch (StripeException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }


        [HttpDelete("cancelsubscription")]
        public async Task<ActionResult<SubscriptionDto>> CancelSubscription(string id)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404));
                }
                var subscription = await _subscriptionService.CancelSubscriptionsAsync(id);

                if (user.Subscription != null)
                {
                    user.Subscription.Id = subscription.Id;
                    user.Subscription.StartDate = subscription.StartDate;
                    user.Subscription.EndDate = subscription.CurrentPeriodEnd;
                    user.Subscription.Status = subscription.Status;
                    user.Subscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
                    user.Subscription.SubscriptionItemId = subscription.Items.Data[0].Id;
                    await _userManager.UpdateAsync(user);
                }

                return _mapper.Map<SubscriptionDto>(subscription);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(40, ex.Message));
            }
        }

        [HttpPost("changesubscriptionplan/{paymentMethodId}/{priceId}")]
        public async Task<ActionResult<SubscriptionDto>> ChangeSubscription(string paymentMethodId, string priceId)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
                if (user?.Subscription == null)
                {
                    return NotFound(new ApiResponse(404));
                }

                var subscription = await _subscriptionService.ChangeSubscriptionAsync(paymentMethodId, user.Subscription.Id, user.Subscription.SubscriptionItemId, priceId);
                var price = await _pricesService.GetPriceAsync(priceId);

                if (user.Subscription != null)
                {
                    user.Subscription.Id = subscription.Id;
                    user.Subscription.StartDate = subscription.StartDate;
                    user.Subscription.EndDate = subscription.CurrentPeriodEnd;
                    user.Subscription.Status = subscription.Status;
                    user.Subscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
                    user.Subscription.SubscriptionItemId = subscription.Items.Data[0].Id;
                    user.Subscription.Price = price.UnitAmountDecimal;
                    user.Subscription.Description = price.Product.Description;
                    user.Subscription.PlanType = price.Product.Name;
                    await _userManager.UpdateAsync(user);
                }

                return _mapper.Map<SubscriptionDto>(subscription);
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
    }


}
