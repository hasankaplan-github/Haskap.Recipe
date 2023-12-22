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

namespace Haskap.Recipe.Application.UseCaseServices.Recipies;
public class StepCreatedEventHandler : INotificationHandler<StepCreatedDomainEvent>
{
    private readonly StepPicturesSettings _stepPicturesSettings;

    public StepCreatedEventHandler(IOptions<StepPicturesSettings> cateringPhotoSettingsOptions)
    {
        _stepPicturesSettings = cateringPhotoSettingsOptions.Value;
    }

    public async Task Handle(StepCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await CreatePhotosAsync(notification, cancellationToken);
    }

    private async Task CreatePhotosAsync(StepCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.FileInputDtos.Any())
        {
            return;
        }

        var fullFolderPath = Path.Combine(
            notification.WebRootPath,
            _stepPicturesSettings.FolderName,
            notification.RecipeId.ToString(),
            notification.NewStepId.ToString());

        Directory.CreateDirectory(fullFolderPath);

        var tasks = notification.FileInputDtos
            .AsParallel()
            .Select(async x =>
            {
                var fullFileName = Path.Combine(fullFolderPath, $"{x.NewName}{x.Extension}");
                using (var fileStream = System.IO.File.Create(fullFileName))
                {
                    await fileStream.WriteAsync(x.Content, cancellationToken);
                }
            });

        await Task.WhenAll(tasks);
    }
}
