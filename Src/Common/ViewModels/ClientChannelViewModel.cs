using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Common.ViewModels
{
    public class ClientChannelViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public ChannelTypeEnum ChannelType { get; set; }
        public int MaxUsers { get; set; }
        public int SessionDuration { get; set; }
        public string SessionDate { get; set; }
        public string SessionEndDate { get; set; }
        public bool GuestLogin { get; set; }
        public int GuestLimit { get; set; }
        public bool OpLoginFirst { get; set; }
        public int BasePrice { get; set; }
        public int BasePriceChange { get; set; }
        public int IntervalPriceI { get; set; }
        public int IntervalPriceII { get; set; }
        public int IntervalPriceIII { get; set; }
        public int IntervalPriceIV { get; set; }
        public string Winner { get; set; }
        public int WinnerPrice { get; set; }
        public int AuctionId { get; set; }
        public IEnumerable<ClientChannelUserMemberModel> UserMembers { get; set; }
        public IEnumerable<ClientChannelUserAuctionModel> UserAuctions { get; set; }
    }
    public class ClientChannelUserMemberModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string NickName { get; set; }
        public bool Joined { get; set; }
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public bool Organizer { get; set; }
    }
    public class ClientChannelUserAuctionModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public int OfferPrice { get; set; }
        public DateTime OfferDate { get; set; }
        public bool OfferAccept { get; set; }
        public string UserId { get; set; }
    }




}
