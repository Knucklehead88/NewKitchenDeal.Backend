using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StripeInvoiceDto
    {

            [Required]
            public string Id { get; set; }

            [StringLength(3)]
            public string Currency { get; set; }
            public string Status { get; set; }
            public string Number { get; set; }
            public string HostedInvoiceUrl { get; set; }
            public string InvoicePdf { get; set; }

            [Required]
            public long AmountPaid { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerPhone { get; set; }
            public string PriceId { get; set; }
            public decimal? UnitAmount { get; set; }
            public DateTime Created { get; set; }
            public DateTime PeriodEnd { get; set; }
            public DateTime PeriodStart { get; set; }

    }
}
