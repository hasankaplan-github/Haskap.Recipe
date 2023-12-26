using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.RecipeAggregate.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class StepDeletedEventHandler : INotificationHandler<StepDeletedDomainEvent>
{
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly IServiceProvider _serviceProvider;

    public StepDeletedEventHandler(
        IOptions<StepPicturesSettings> cateringPhotoSettingsOptions,
        IServiceProvider serviceProvider)
    {
        _stepPicturesSettings = cateringPhotoSettingsOptions.Value;
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(StepDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await DeleteStepPicturesFolder(notification, cancellationToken);
    }

    private async Task DeleteStepPicturesFolder(StepDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var fullFolderName = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString(),
            notification.StepId.ToString());

        if (Directory.Exists(fullFolderName))
        {
            Directory.Delete(fullFolderName, recursive: true);
        }
    }
}
