using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextInformationSeed
    {
        public static async Task SeedTradesAndLanguagesAsync(AppIdentityDbContext appIdentityDbContext)
        {
            if (!appIdentityDbContext.Trades.Any())
            {
                var trades = new List<Trade>()
                {
                    new() { Name = "Powerwashing", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"]},
                    new() { Name = "Cleaning", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Laborer", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Holiday Decorations and Christmas Lights", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Demolition", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Gutter Cleaning", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Trash Removal and Cleanout", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Dumpster", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Landscaping", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Handyman", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Moving", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Local", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "National", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Delivery", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Material", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Grocery", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Box Truck", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Semi", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Liftgate", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Dog Walking", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Notary", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Mobile", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Credit Repair", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Homeowners Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "General Liability Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Builder�s Risk Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZros"] },
                    new() { Name = "Life Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Auto Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Health Insurance", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Furniture Assembly / Mounting", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "TV Mounting", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Furniture Assembly", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Hang Pictures", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Waiting In Line", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Appliance Repair", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Chimney", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Sweeping / Cleaning", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Liner Replacement", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Garage Door Repair/Installation", Categories = ["price_1PAUu1DOwUvKzODD9UzHOLZr"] },
                    new() { Name = "Accounting and Tax", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Accountant", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Tax Preparation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Bookkeeper", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Architect", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Carpenter", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Finish Carpentry", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Rough Carpentry / Framing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Custom Stairs", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Custom Cabinets", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Custom Doors", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Decking", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"]},
                    new() { Name = "Duct Cleaning", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Dredger", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Drywall", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Electrician", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Fencing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Flooring", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Framer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "General Contractor", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Heating and Cooling (HVAC)", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Boiler", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Furnace and Air Conditioning", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Insulation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Lead Paint", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Testing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Remediation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Lawyer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Categories", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Mason", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Painter", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Pipe Cleaning / Pipe Clearing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Plasterer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Plumber", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Sewer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Quantity Surveillance", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Radon", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Testing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Remediation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Mitigation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Roofer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Flat", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Shingle", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Roll", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Metal", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Scaffolding", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Safety, Health, and Environmental Engineer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Siding", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Sheet metal worker", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Structural Engineer", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Taping", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Tiler", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Welder", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Windows", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Lender", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Hard money", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Business Loans", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Home Loans", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Commercial Real Estate Loans", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "SBA Loans", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Concrete", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Fencing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Gutters", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Cleaning", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Installation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Mold Remediation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Pest Control", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Commercial", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Residential", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Termite", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Septic", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Maintenance", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Installation", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Tree Removal", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Trimming", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Stump Removal", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Welding", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Hardwood Floor Refinishing", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
					new() { Name = "Countertops", Categories = ["price_1PAUlqDOwUvKzODDO9xZ3LYZ", "price_1PAURgDOwUvKzODDUC7Iv9XA"] },
                };

                await appIdentityDbContext.Trades.AddRangeAsync(trades);
                await appIdentityDbContext.SaveChangesAsync();

            }

            if (!appIdentityDbContext.Languages.Any())
            {
                var trades = new List<Language>()
                {
                    new() { Name = "English" },
                    new() { Name = "Spanish" },
                    new() { Name = "Chinese" },
                    new() { Name = "Italian" },
                    new() { Name = "French" },
                    new() { Name = "German" },
                    new() { Name = "Japanese" },
                    new() { Name = "Hindi" },
                    new() { Name = "Bengali" },
                    new() { Name = "Turkish" },
                    new() { Name = "Russian" },
                    new() { Name = "Portuguese" },
                    new() { Name = "Arabic" },
                    new() { Name = "Romanian" },
                    new() { Name = "Dutch" },
                };

                await appIdentityDbContext.Languages.AddRangeAsync(trades);
                await appIdentityDbContext.SaveChangesAsync();

            }
        }
    }
}