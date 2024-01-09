namespace XploringMe.BlazorApp.Components.Pages.Categories;

public partial class CategoryPage
{
    protected CategoryDto       Model       { get; set; } = default!;
    protected List<CategoryDto> Categories  { get; set; } = [];
    protected bool              EditingForm { get; set; }

    protected Validations   CustomValidation    = new();
    protected bool          Loading             = true;

    protected override async Task OnInitializedAsync()
    {
        await GetLatestData();
        await base.OnInitializedAsync();
    }

    private async Task GetLatestData()
    {
        var catResult = CatRepo.FindAll(e => e.DeletedOn == null, orderBy: e => e.OrderBy("IsActive").ThenBy("Name"));
        if (catResult.HasErrors)
        {
            var error = catResult.GetErrors();
            await NotificationService.Error(error);
            return;
        }
        var categories = catResult.ResultObject.ToList();
        Categories = Mapper.Map<List<CategoryDto>>(categories);
        Loading = false;
        Model = new CategoryDto();
        StateHasChanged();
    }
    protected async Task Submit()
    {
        Loading = true;
        if (!await CustomValidation.ValidateAll())
        {
            Loading = false;
            return;
        }
        var ifExists = CatRepo.FindAll(e => e.Name == Model.Name && e.Type == Model.Type).ResultObject.Any();
        if (ifExists)
        {
            await NotificationService.Error($"Category {Model.Name} already exists");
            Loading = false;
            return;
        }
        var category    = Mapper.Map<Category>(Model);
        var result      = CatRepo.Create(category, 1);
        if (result.HasErrors)
        {
            await NotificationService.Error($"Error : {result.GetErrors()}");
            Loading = false;
            return;
        }
        await NotificationService.Success("Category created successfully");
        await GetLatestData();
    }

    protected async Task Update()
    {
        Loading = true;
        if (!await CustomValidation.ValidateAll())
        {
            Loading = false;
            return;
        }
        var ifExists = CatRepo.FindAll(e => e.Name == Model.Name && e.Type == Model.Type && e.Id != Model.Id)
                              .ResultObject.Any();
        if (ifExists)
        {
            await NotificationService.Error($"Category {Model.Name} already exists");
            EditingForm = true;
            Loading = false;
            return;
        }
        var category = CatRepo.FindAll(e => e.Id == Model.Id).ResultObject.FirstOrDefault();
        if (category is null)
        {
            await NotificationService.Error($"Category {Model.Name} not exists");
            EditingForm = true;
            Loading = false;
            return;
        }
     
        category.Name           = Model.Name;
        category.IsActive       = Model.IsActive;
        category.Color          = Model.Color;
        category.Type           = Model.Type;
        category.Description    = Model.Description;
      
        var result = CatRepo.Update(category, 1);
        if (result.HasErrors)
        {
            await NotificationService.Error($"Error : {result.GetErrors()}");
            EditingForm = true;
            Loading = false;
            return;
        }
        await NotificationService.Success("Category updated successfully");
        EditingForm = false;
        await GetLatestData();
    }

    protected async Task Delete(long id)
    {
        if (id > 0)
        {
            Loading = true;
            var category = CatRepo.FindAll(e => e.Id == id).ResultObject.FirstOrDefault();
            if (category is null)
            {
                await NotificationService.Error($"Category {Model.Name} not exists");
                Loading = false;
                return;
            }
            var result = CatRepo.Delete(category, 1);
            if (result.HasErrors)
            {
                await NotificationService.Error($"Error : {result.GetErrors()}");
                Loading = false;
                return;
            }
            await NotificationService.Success("Category deleted successfully");
            await GetLatestData();
            Loading = false;
        }
    }
}
