using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.Handlers;
using Coreapi.Domain.ValueObjects;

namespace Coreapi.Domain.AggregatesModel.ClientAggregate
{
    public class Client
    {
        private Client() { }

        public Client(string name, string abn,
            CompanyTypeEnum companyType, StaffRangeEnum staffRange)
        {
            Name = name;
            Subdomain = name.ToLower().Trim();
            ABN = abn;
            CompanyType = companyType;
            StaffRange = staffRange;
            ApiKey = Helper.GenerateApiKey();
            Id = Guid.NewGuid();
            Setting = new ClientSetting("Welcome", "Goodbye", "Break In", "Break Out"
                , 60, 480, 20, 4, DayOfWeek.Saturday, DayOfWeek.Sunday);
            Balance = 0;

            Analyzers = new Collection<ClientAnalyzer>();
            UserSettings = new Collection<ClientUserSetting>();
            UserTrainings = new Collection<ClientUserTraining>();
            Cards = new Collection<ClientCard>();
            Areas = new Collection<ClientArea>();
        }

        public void UpdateChatInfo(string chatUrl, string token, string rocketChatId)
        {
            ChatUrl = chatUrl;
            RocketChatToken = token;
            RocketChatId = rocketChatId;
        }

        public void UpdateApiKey()
        {
            ApiKey = Helper.GenerateApiKey();
        }

        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Subdomain { get; init; }
        public string ABN { get; init; }
        public CompanyTypeEnum CompanyType { get; init; }
        public StaffRangeEnum StaffRange { get; init; }
        public string ChatUrl { get; private set; }
        public string RocketChatToken { get; private set; }
        public string RocketChatId { get; private set; }
        public string ApiKey { get; private set; }
        public decimal Balance { get; private set; }
        public ClientSetting Setting { get; private set; }
        public void SetStting(ClientSetting setting)
        {
            if (setting is not null)
                Setting = setting;
        }

        public void IncreaseBalance(decimal balance) => Balance += balance;
        public void DecreaseBalance(decimal balance) => Balance -= balance;

        public ICollection<ClientUserSetting> UserSettings { get; init; }
        public IStatusHandler AddUserSetting(ClientUserSetting userSetting)
        {
            if (userSetting is null)
                throw new ArgumentNullException(nameof(userSetting));

            var status = new StatusHandler();
            if (UserSettings is null)
            {
                status.AddError(
                    "Must first retrive the User Settings list",
                    nameof(ClientUserSetting));
                return status;
            }

            if(UserSettings.Any(u=>u.UserId.Equals(userSetting.UserId)))
            {
                status.AddError(
                    "Duplicate User Id",
                    userSetting.UserId);
                return status;
            }

            UserSettings.Add(userSetting);
            return status;
        }

      
        public ICollection<ClientArea> Areas { get; init; }
        public IStatusHandler AddArea(string name, string description, IEnumerable<Point> area)
        {
            ArgumentNullException.ThrowIfNull(area, nameof(area));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var status = new StatusHandler();
            if (Areas is null)
            {
                status.AddError(
                    "Must first retrive the Areas list",
                    nameof(ClientArea));
                return status;
            }

            Areas.Add(new ClientArea(name, description, area));
            return status;
        }

        public IStatusHandler AddArea(string name, string description, double circleRadius, Point circleCenter)
        {
            ArgumentNullException.ThrowIfNull(circleCenter, nameof(circleCenter));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var status = new StatusHandler();
            if (Areas is null)
            {
                status.AddError(
                    "Must first retrive the Areas list",
                    nameof(ClientArea));
                return status;
            }

            Areas.Add(new ClientArea(name, description, circleRadius, circleCenter));
            return status;
        }

        public IStatusHandler RemoveArea(ClientArea clientArea)
        {
            ArgumentNullException.ThrowIfNull(clientArea, nameof(clientArea));

            var status = new StatusHandler();
            if (Areas is null)
            {
                status.AddError(
                    "Must first retrive the Areas list",
                    nameof(ClientArea));
                return status;
            }

            Areas.Remove(clientArea);
            return status;
        }

        public ICollection<ClientAnalyzer> Analyzers { get; init; }
        public IStatusHandler AddAnalyzer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var status = new StatusHandler();
            if (Analyzers is null)
            {
                status.AddError(
                    "Must first retrive the Analyzers list",
                    nameof(ClientAnalyzer));
                return status;
            }

            if (!Analyzers.Any(a => a.UserId.Equals(userId)))
                Analyzers.Add(new ClientAnalyzer(userId));

            return status;
        }

        public IStatusHandler RemoveAnalyzer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var status = new StatusHandler();
            if (Analyzers is null)
            {
                status.AddError(
                    "Must first retrive the Analyzers list",
                    nameof(ClientAnalyzer));
                return status;
            }

            var analyzer = Analyzers.FirstOrDefault(a => a.UserId.Equals(userId));

            if (analyzer is not null)
                Analyzers.Remove(analyzer);

            return status;
        }

        public ICollection<ClientUserTraining> UserTrainings { get; init; }
        public IStatusHandler AddTraining(string userId, string title, string link)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            if (string.IsNullOrEmpty(link))
                throw new ArgumentNullException(nameof(link));

            var status = new StatusHandler();
            if (UserTrainings is null)
            {
                status.AddError(
                    "Must first retrive the Trainings list",
                    nameof(ClientUserTraining));
                return status;
            }

            UserTrainings.Add(new ClientUserTraining(userId, title, link));
            return status;
        }

        public IStatusHandler RemoveTraining(ClientUserTraining userTraining)
        {
            if (userTraining is null)
                throw new ArgumentNullException(nameof(userTraining));

            var status = new StatusHandler();
            if (UserTrainings is null)
            {
                status.AddError(
                    "Must first retrive the Trainings list",
                    nameof(ClientUserTraining));
                return status;
            }
            
            UserTrainings.Remove(userTraining);
            return status;
        }

        public ICollection<ClientCard> Cards { get; init; }
        public IStatusHandler AddCard(string nameOnCard, string cardNumber, CardTypeEnum type, string cCV, string expireDate)
        {
            if (string.IsNullOrEmpty(nameOnCard))
                throw new ArgumentNullException(nameof(nameOnCard));

            if (string.IsNullOrEmpty(cardNumber))
                throw new ArgumentNullException(nameof(cardNumber));

            if (string.IsNullOrEmpty(cCV))
                throw new ArgumentNullException(nameof(cCV));

            if (string.IsNullOrEmpty(expireDate))
                throw new ArgumentNullException(nameof(expireDate));

            var status = new StatusHandler();
            if (Cards is null)
            {
                status.AddError(
                    "Must first retrive the Cards list",
                    nameof(ClientCard));
                return status;
            }

            Cards.Add(new ClientCard(nameOnCard, cardNumber, type, cCV, expireDate));
            return status;
        }

        public IStatusHandler RemoveCard(Guid id)
        {
            var status = new StatusHandler();
            if (Cards is null)
            {
                status.AddError(
                    "Must first retrive the Cards list",
                    nameof(ClientCard));
                return status;
            }

            var card = Cards.FirstOrDefault(c => c.Id == id);
            if(card is null)
            {
                status.AddError(
                    "Card Id Not Exist",
                    id.ToString());
                return status;
            }

            Cards.Remove(card);
            return status;
        }
    }
}
