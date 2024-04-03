using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

            CreateMap<PersonalInfo, PersonalInfoDto>()
                .ForMember(a => a.Locations, opt => opt.Ignore());
                //.ForMember(a => a.ProfilePictureImage, opt => opt.Ignore());


            CreateMap<PersonalInfoDto, PersonalInfo>()
                .ForMember(a => a.Locations, opt => opt.Ignore());

            CreateMap<PersonalInfo, ResponsePersonalInfoDto>()
                .ForMember(a => a.Locations, opt => opt.Ignore());

            CreateMap<ResponsePersonalInfoDto, PersonalInfo>()
                .ForMember(a => a.Locations, opt => opt.Ignore());

            CreateMap<LocationDto, Location>()
                .ForMember(a => a.Id, opt => opt.Ignore())
                .ReverseMap();

            //CreateMap<Location, LocationDto>()
            //    .ForSourceMember(a => a.Point, opt => opt.DoNotValidate());

            CreateMap<ExternalAuthDto, ExternalAuth>().ReverseMap();
            CreateMap<BusinessInfoDto, BusinessInfo>()
                .ForMember(a => a.Trades, opt => opt.Ignore())
                .ForMember(a => a.SpokenLanguages, opt => opt.Ignore())
                .ForMember(a => a.Locations, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<BusinessInfo, ResponseBusinessInfoDto>()
                .ForMember(a => a.Trades, opt => opt.Ignore())
                .ForMember(a => a.SpokenLanguages, opt => opt.Ignore())
                .ForMember(a => a.Locations, opt => opt.Ignore());

            CreateMap<ResponseBusinessInfoDto, BusinessInfo>()
                .ForMember(a => a.Trades, opt => opt.Ignore())
                .ForMember(a => a.SpokenLanguages, opt => opt.Ignore());

            CreateMap<Trade, TradeDto>()
                .ReverseMap();
            CreateMap<Trade, ResponseTradeDto>()
                .ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Language, ResponseLanguageDto>().ReverseMap();

            
            CreateMap<Stripe.Product, StripeProductDto>().ReverseMap();
            CreateMap<Stripe.Price, StripePriceDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
                .ForMember(d => d.Interval, o => o.MapFrom(s => s.Recurring.Interval))
                .ForMember(d => d.UnitAmount, o => o.MapFrom(s => s.UnitAmount))
                .ReverseMap();
            CreateMap<Stripe.Customer, StripeCustomerDto>()
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
                .ReverseMap();

            CreateMap<CreatePaymentMethod, CreatePaymentMethodDto>().ReverseMap();
            CreateMap<Stripe.PaymentMethod, PaymentMethodDto>()
                .ForMember(d => d.Last4, o => o.MapFrom(s => s.Card.Last4))
                .ForMember(d => d.ExpMonth, o => o.MapFrom(s => s.Card.ExpMonth))
                .ForMember(d => d.ExpYear, o => o.MapFrom(s => s.Card.ExpYear))
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Card.Brand))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.BillingDetails.Name))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.BillingDetails.Phone))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.BillingDetails.Email))
                .ReverseMap();
            CreateMap<SetupIntentDto, Stripe.SetupIntent>()
                .ReverseMap();

            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<Stripe.Card, CardDto>()
                .ReverseMap();

            CreateMap<Stripe.Subscription, PostSubscriptionDto>()
                .ForMember(d => d.CustomerId, o => o.MapFrom(s => s.CustomerId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.PriceId, o => o.MapFrom(s => s.Items.Data[0].Price.Id))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Items.Data[0].Price.ProductId))
                .ForMember(d => d.UnitAmount, o => o.MapFrom(s => s.Items.Data[0].Price.UnitAmountDecimal))
                .ReverseMap();

            CreateMap<Stripe.Subscription, SubscriptionDto>()
                .ForMember(d => d.SubscriptionItemId, o => o.MapFrom(s => s.Items.Data[0].Id))
                .ReverseMap();

            CreateMap<Subscription, SubscriptionDto>()
                .ReverseMap();

            CreateMap<Stripe.Invoice, StripeInvoiceDto>()
                .ForMember(d => d.PriceId, o => o.MapFrom(s => s.Subscription.Items.FirstOrDefault().Price.Id))
                .ForMember(d => d.UnitAmount, o => o.MapFrom(s => s.Subscription.Items.FirstOrDefault().Price.UnitAmountDecimal))
                .ReverseMap();

            CreateMap<SendToEmail, SendToEmailDto>()
                .ReverseMap();

        }
    }
}