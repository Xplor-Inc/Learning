using AutoMapper;

namespace XploringMe.BlazorApp.Models;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Enquiry,              EnquiryDto>()
            .ReverseMap();

        CreateMap<User,                 UserDto>()
            .ReverseMap();

        CreateMap<Counter,              CounterDto>()
            .ReverseMap();
        
        
        //Financial Management
        CreateMap<Category,             CategoryDto>()
           .ReverseMap();
        CreateMap<TransactionAccount,   TransactionAccountDto>()
           .ReverseMap();
        CreateMap<Transaction,          TransactionDto>()
           .ReverseMap();
        CreateMap<RefundHistory,        RefundHistoryDto>()
           .ReverseMap();
        CreateMap<Budget,               BudgetDto>()
           .ReverseMap();
        CreateMap<RecurringBill,        RecurringBillDto>()
           .ReverseMap();
        CreateMap<TransactionTagging,   TransactionTaggingDto>()
          .ReverseMap();

        //// Inverstments
        //CreateMap<DematAccount,         DematAccountDto>()
        // .ReverseMap();
        //CreateMap<Scrip,                ScripDto>()
        // .ReverseMap();
        //CreateMap<Stock,                CreateStockDto>()
        // .ReverseMap();
        //CreateMap<Stock,                StockDto>()
        // .ReverseMap();
        //CreateMap<SchemeInvestment,           SchemeInvestmentDto>()
        // .ReverseMap();
        //CreateMap<Scheme,               SchemeDto>()
        // .ReverseMap();
        //CreateMap<Scheme,               CreateSchemeDto>()
        // .ReverseMap();
    }
}
