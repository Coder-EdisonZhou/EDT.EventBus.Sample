﻿using EDT.MSA.API.Shared.Events;
using EDT.MSA.API.Shared.Models;
using System.Threading.Tasks;

namespace EDT.MSA.Stocking.API.Services
{
    public interface INewOrderSubmittedEventService
    {
        Task<EventData<ProductStockDeductedEvent>> DeductProductStock(EventData<NewOrderSubmittedEvent> eventData); 
    }
}
