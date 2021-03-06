﻿using System;
using System.Threading.Tasks;
using Marketplace.Contracts;
using Marketplace.Domain.ClassifiedAds;
using Marketplace.Framework;

namespace Marketplace
{
    public class ClassifiedAdsApplicationService
    {
        private readonly IAggregateStore _store;

        public ClassifiedAdsApplicationService(IAggregateStore store)
        {
            _store = store;
        }

        public Task Handle(ClassifiedAds.V1.Create command) =>
            _store.Save(ClassifiedAd.Create(
                command.Id,
                command.OwnerId,
                command.CreatedBy));

        public Task Handle(ClassifiedAds.V1.RenameAd command) =>
            HandleUpdate(command.Id, ad =>
                ad.Rename(command.Title, command.RenamedBy)
            );

        public Task Handle(ClassifiedAds.V1.UpdateText command) =>
            HandleUpdate(command.Id, ad =>
                ad.UpdateText(command.Text, command.TextChangedBy)
            );

        public Task Handle(ClassifiedAds.V1.ChangePrice command) =>
            HandleUpdate(command.Id, ad =>
                ad.ChangePrice(command.Price, command.PriceChangedBy)
            );

        public Task Handle(ClassifiedAds.V1.Publish command) =>
            HandleUpdate(command.Id, ad =>
                ad.Publish(command.PublishedBy)
            );

        public Task Handle(ClassifiedAds.V1.Activate command) =>
            HandleUpdate(command.Id, ad =>
                ad.Activate(command.ActivatedBy)
            );

        public Task Handle(ClassifiedAds.V1.Report command) =>
            HandleUpdate(command.Id, ad =>
                ad.Report(command.Reason, command.ReportedBy)
            );

        public Task Handle(ClassifiedAds.V1.Reject command) =>
            HandleUpdate(command.Id, ad =>
                ad.Reject(command.Reason, command.RejectedBy)
            );

        public Task Handle(ClassifiedAds.V1.Deactivate command) =>
            HandleUpdate(command.Id, ad =>
                ad.Deactivate(command.DeactivatedBy)
            );

        public Task Handle(ClassifiedAds.V1.MarkAsSold command) =>
            HandleUpdate(command.Id, ad =>
                ad.MarkAsSold(command.MarkedBy)
            );

        public Task Handle(ClassifiedAds.V1.Remove command) =>
            HandleUpdate(command.Id, ad =>
                ad.Remove(command.RemovedBy)
            );

        private async Task HandleUpdate(Guid id, Action<ClassifiedAd> update)
        {
            var ad = await _store.Load<ClassifiedAd>(id.ToString());
            update(ad);
            await _store.Save(ad);
        }
    }
}
