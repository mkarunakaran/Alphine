using Sumday.Infrastructure.Surpas.ShareHolder.Plans;

namespace Sumday.Service.ShareHolder.Accounts.ViewModels
{
    public class AccountGift
    {
        public PlanGiftMode Mode { get; set; }

        public string AssetId { get; set; }

        public decimal ModeAmount { get; set; }
    }
}
